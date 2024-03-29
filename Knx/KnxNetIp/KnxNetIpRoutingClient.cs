﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Knx.KnxNetIp.MessageBody;
using Microsoft.Extensions.Logging;

namespace Knx.KnxNetIp;

/// <summary>
///     Used to connect to the Knx Bus via KnxNetIpRouting protocol.
/// </summary>
public sealed class KnxNetIpRoutingClient : IKnxNetIpClient, IObservable<KnxHpai>
{
    private readonly KnxDeviceAddress _deviceAddress;
    private readonly IPEndPoint _localEndpoint;
    private readonly ILogger<KnxNetIpRoutingClient> _logger;
    private readonly CancellationTokenSource _receivingMessagesCancellationTokenSource;
    private readonly IPEndPoint _remoteEndPoint;
    private UdpClient? _udpClient;

    private readonly Subject<KnxNetIpMessage> _knxNetIpMessageSubject;
    private readonly Subject<IKnxMessage> _knxMessageSubject;
    private readonly Subject<KnxHpai> _knxHpaiSubject;

    /// <summary>
    ///     Creates a new instance of the <see cref="KnxNetIpRoutingClient" /> class.
    /// </summary>
    /// <param name="configureOptions">Configuration builder</param>
    /// ///
    /// <param name="logger">Optional logger</param>
    public KnxNetIpRoutingClient(
        Action<KnxNetIpRoutingClientOptions>? configureOptions = null,
        ILogger<KnxNetIpRoutingClient>? logger = null)
    {
        _logger = logger ?? NullLogger<KnxNetIpRoutingClient>.Instance;

        var options = new KnxNetIpRoutingClientOptions();
        configureOptions?.Invoke(options);

        _remoteEndPoint = new IPEndPoint(options.RemoteAddress, options.RemotePort);
        _localEndpoint = new IPEndPoint(IPAddress.Any, options.RemotePort);
        _deviceAddress = options.DeviceAddress;

        _receivingMessagesCancellationTokenSource = new CancellationTokenSource();

        _knxNetIpMessageSubject = new Subject<KnxNetIpMessage>();
        _knxMessageSubject = new Subject<IKnxMessage>();
        _knxHpaiSubject = new Subject<KnxHpai>();
    }

    public async ValueTask DisposeAsync()
    {
        await Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Occurs when [KNX message received].
    /// </summary>
    public event EventHandler<IKnxMessage>? KnxMessageReceived;

    /// <summary>
    ///     Sends a KnxMessage.
    /// </summary>
    public async Task SendMessageAsync(IKnxMessage knxMessage, CancellationToken cancellationToken = default)
    {
        knxMessage.SourceAddress ??= _deviceAddress;
        var knxNetIpMessage = KnxNetIpMessage.Create(KnxNetIpServiceType.RoutingIndication);
        ((RoutingIndication)knxNetIpMessage.Body!).Cemi = knxMessage;

        await SendMessageAsync(knxNetIpMessage, cancellationToken);
    }

    /// <summary>
    ///     Connects this instance.
    /// </summary>
    public async Task ConnectAsync()
    {
        _udpClient = new UdpClient
        {
            MulticastLoopback = false,
            ExclusiveAddressUse = false
        };

        _udpClient.Client.SetSocketOption(
            SocketOptionLevel.Socket,
            SocketOptionName.ReuseAddress,
            true);

        _udpClient.Client.Bind(_localEndpoint);
        _udpClient.JoinMulticastGroup(_remoteEndPoint.Address);

        _ = ReceiveMessagesAsync(_receivingMessagesCancellationTokenSource.Token);

        await Task.CompletedTask;
    }

    IDisposable IObservable<KnxNetIpMessage>.Subscribe(IObserver<KnxNetIpMessage> observer) =>
        _knxNetIpMessageSubject.Subscribe(observer);

    IDisposable IObservable<IKnxMessage>.Subscribe(IObserver<IKnxMessage> observer) =>
        _knxMessageSubject.Subscribe(observer);

    IDisposable IObservable<KnxHpai>.Subscribe(IObserver<KnxHpai> observer) =>
        _knxHpaiSubject.Subscribe(observer);

    ~KnxNetIpRoutingClient()
    {
        _ = Dispose(false);
    }

    private ValueTask Dispose(bool disposing)
    {
        if (disposing)
        {
            _receivingMessagesCancellationTokenSource.Cancel();
            _udpClient?.Dispose();
            _udpClient = null;

            _knxNetIpMessageSubject.Dispose();
            _knxMessageSubject.Dispose();
            _knxHpaiSubject.Dispose();
        }

        return ValueTask.CompletedTask;
    }

    /// <summary>
    ///     Called when knx device has been discovered.
    /// </summary>
    public event EventHandler<KnxHpai>? KnxDeviceDiscovered;

    private async Task ReceiveMessagesAsync(CancellationToken cancellationToken)
    {
        var receivedBuffer = new List<byte>();

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                var udpReceiveResult = await _udpClient!.ReceiveAsync(cancellationToken);
                var receivedData = udpReceiveResult.Buffer.ToArray();
                receivedBuffer.AddRange(receivedData);

                var knxNetIpMessage = KnxNetIpMessage.Parse(receivedBuffer.ToArray());
                OnKnxNetIpMessageReceived(knxNetIpMessage);

                receivedBuffer.Clear();
            }
            catch (Exception exception) when (
                exception is ObjectDisposedException
                    or OperationCanceledException)
            {
                // UdpClient has been disposed or cancelled => stop listening
                break;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while receiving message");
            }
            finally
            {
                receivedBuffer.Clear();
            }
    }

    /// <summary>
    ///     Sends a KnxNetIpMessage
    /// </summary>
    private async Task SendMessageAsync(KnxNetIpMessage message, CancellationToken cancellationToken)
    {
        EnsureConnectionEstablished();

        try
        {
            var bytes = message.ToByteArray();
            await _udpClient!.SendAsync(bytes, _remoteEndPoint, cancellationToken);

            _logger.LogInformation("Send: {Message}", message);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error sending message: {message}", message);

            throw;
        }
    }

    public async Task DiscoverAsync(CancellationToken cancellationToken = default)
    {
        await SendMessageAsync(
            MessageFactory.GetSearchRequest(_remoteEndPoint),
            cancellationToken);
    }

    private void EnsureConnectionEstablished()
    {
        if (_udpClient is null)
            throw new KnxNetIpException("Client is not connected");
    }

    private void OnKnxNetIpMessageReceived(KnxNetIpMessage message)
    {
        try
        {
            switch (message.Body)
            {
                case SearchResponse searchResponse:
                    InvokeKnxDeviceDiscovered(searchResponse.Endpoint);
                    break;
                case RoutingIndication routingIndication:
                    InvokeKnxMessageReceived(routingIndication.Cemi);
                    break;
            }

            _logger.LogInformation("Received: {Message}", message);
        }
        catch (Exception e)
        {
            throw new KnxNetIpException("Error while handling received message", e);
        }

        _knxNetIpMessageSubject.OnNext(message);
    }

    private void InvokeKnxMessageReceived(IKnxMessage knxMessage)
    {
        KnxMessageReceived?.Invoke(this, knxMessage);
        _knxMessageSubject.OnNext(knxMessage);
    }

    private void InvokeKnxDeviceDiscovered(KnxHpai hostProtocolAddressInformation)
    {
        KnxDeviceDiscovered?.Invoke(this, hostProtocolAddressInformation);
        _knxHpaiSubject.OnNext(hostProtocolAddressInformation);
    }
}
