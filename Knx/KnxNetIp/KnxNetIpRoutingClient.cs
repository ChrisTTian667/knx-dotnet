using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Knx.KnxNetIp.MessageBody;

namespace Knx.KnxNetIp;

/// <summary>
///     Used to connect to the Knx Bus via KnxNetIpRouting protocol.
/// </summary>
public sealed class KnxNetIpRoutingClient : IKnxNetIpClient, IDisposable
{
    private UdpClient? _udpClient;
    private readonly IPEndPoint _remoteEndPoint;
    private readonly KnxDeviceAddress _deviceAddress;
    private readonly IPEndPoint _localEndpoint;
    private readonly CancellationTokenSource _receivingMessagesCancellationTokenSource;

    /// <summary>
    ///     Creates a new instance of the <see cref="KnxNetIpRoutingClient"/> class.
    /// </summary>
    /// <param name="configureOptions">Configuration builder</param>
    public KnxNetIpRoutingClient(Action<KnxNetIpRoutingClientOptions>? configureOptions = null)
    {
        var options = new KnxNetIpRoutingClientOptions();
        configureOptions?.Invoke(options);

        _remoteEndPoint = new IPEndPoint(options.RemoteAddress, options.RemotePort);
        _localEndpoint = new IPEndPoint(IPAddress.Any, options.RemotePort);
        _deviceAddress = options.DeviceAddress;

        _receivingMessagesCancellationTokenSource = new CancellationTokenSource();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~KnxNetIpRoutingClient() =>
        Dispose(false);

    private void Dispose(bool disposing)
    {
        if (!disposing)
            return;

        _receivingMessagesCancellationTokenSource.Cancel();
        _udpClient?.Dispose();
        _udpClient = null;
    }

    /// <summary>
    ///     Occurs when [KNX message received].
    /// </summary>
    public event EventHandler<IKnxMessage>? KnxMessageReceived;

    /// <summary>
    ///     Called when knx device has been discovered.
    /// </summary>
    public event EventHandler<DeviceInfo>? KnxDeviceDiscovered;

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

    private async Task ReceiveMessagesAsync(UdpClient client, CancellationToken cancellationToken)
    {
        var receivedBuffer = new List<byte>();

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var udpReceiveResult = await client.ReceiveAsync(cancellationToken);
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
            catch (Exception e)
            {
                // TODO: Handle parsing error! Just logging?!
                Console.WriteLine(e);
            }
            finally
            {
                receivedBuffer.Clear();
            }
        }
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

        _ = ReceiveMessagesAsync(
            _udpClient,
            _receivingMessagesCancellationTokenSource.Token);

        await Task.CompletedTask;
    }

    /// <summary>
    ///     Sends a KnxNetIpMessage
    /// </summary>
    public async Task SendMessageAsync(KnxNetIpMessage message, CancellationToken cancellationToken)
    {
        EnsureConnectionEstablished();

        Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} SEND => {message}");

        var bytes = message.ToByteArray();

        await _udpClient!.SendAsync(bytes, _remoteEndPoint, cancellationToken);
    }

    public async Task DiscoverAsync(CancellationToken cancellationToken = default) =>
        await SendMessageAsync(
            MessageFactory.GetSearchRequest(_remoteEndPoint),
            cancellationToken);

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
                    InvokeKnxDeviceDiscovered(
                        CreateDeviceInfoFromKnxHpai(
                            searchResponse.Endpoint,
                            searchResponse.Endpoint.Description.FriendlyName));

                    break;
                case RoutingIndication routingIndication:
                    InvokeKnxMessageReceived(routingIndication.Cemi);

                    break;
            }

            //Console.WriteLine("{0} RECV <= {1} (HANDLED)", DateTime.Now.ToLongTimeString(), message);
        }
        catch (Exception e)
        {
            throw new KnxNetIpException("Error while handling received message", e);
        }
    }

    private void InvokeKnxMessageReceived(IKnxMessage knxMessage) =>
        KnxMessageReceived?.Invoke(this, knxMessage);

    private void InvokeKnxDeviceDiscovered(DeviceInfo knxDeviceInfo) =>
        KnxDeviceDiscovered?.Invoke(this, knxDeviceInfo);

    private static DeviceInfo CreateDeviceInfoFromKnxHpai(KnxHpai endpoint, string friendlyName)
    {
        var connection = new KnxNetIpConnectionString
        {
            InternalAddress = $"{endpoint.IpAddress}:{endpoint.Port}",
            DeviceMain = endpoint.Description.Address.Area,
            DeviceMiddle = endpoint.Description.Address.Line,
            DeviceSub = endpoint.Description.Address.Device
        };

        return new DeviceInfo(friendlyName, connection.ToString());
    }
}
