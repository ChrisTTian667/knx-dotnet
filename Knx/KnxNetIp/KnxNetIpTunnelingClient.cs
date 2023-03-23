using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Knx.Exceptions;
using Knx.KnxNetIp.MessageBody;
using Microsoft.Extensions.Logging;

namespace Knx.KnxNetIp;

/// <summary>
///     Used to connect to the Knx Bus via KnxNetIpTunneling protocol.
/// </summary>
public sealed class KnxNetIpTunnelingClient : IKnxNetIpClient
{
    private readonly object _communicationChannelLock = new();
    private readonly KnxDeviceAddress _deviceAddress;
    private readonly ILogger<KnxNetIpRoutingClient> _logger;
    private readonly KnxNetIpTunnelingClientOptions _options;

    private readonly CancellationTokenSource _receivingMessagesCancellationTokenSource;

    private readonly IPEndPoint _remoteEndPoint;

    private readonly SemaphoreSlim _sendSemaphoreSlim = new(1, 1);
    //private readonly AutoResetEvent _terminationEvent = new(false);

    private readonly UdpClient _udpClient;
    private DateTime _connectionStateTimeStamp;
    private byte? _currentCommunicationChannel;

    private bool _isDisposed;

    //private bool _logicalConnected;
    private byte _sequenceCounter;

    public KnxNetIpTunnelingClient(
        Action<KnxNetIpTunnelingClientOptions>? configureOptions = null,
        ILogger<KnxNetIpRoutingClient>? logger = null)
    {
        _logger = logger ?? NullLogger<KnxNetIpRoutingClient>.Instance;

        _options = new KnxNetIpTunnelingClientOptions();
        configureOptions?.Invoke(_options);

        _deviceAddress = _options.DeviceAddress;
        _remoteEndPoint = new IPEndPoint(_options.RemoteAddress, _options.RemotePort);
        _udpClient = new UdpClient();

        _receivingMessagesCancellationTokenSource = new CancellationTokenSource();
    }

    /// <summary>
    ///     Gets or sets the current communication channel.
    /// </summary>
    /// <value>The communication channel.</value>
    private byte? CommunicationChannel
    {
        get
        {
            lock (_communicationChannelLock)
            {
                return _currentCommunicationChannel;
            }
        }
        set
        {
            lock (_communicationChannelLock)
            {
                _currentCommunicationChannel = value;
            }
        }
    }

    /// <summary>
    ///     Gets a value indicating whether this instance timed out.
    /// </summary>
    public bool IsTimedOut => _connectionStateTimeStamp < DateTime.Now.Subtract(TimeSpan.FromSeconds(61));

    public async ValueTask DisposeAsync()
    {
        if (!_isDisposed)
        {
            _isDisposed = true;

            await DisposeAsync(true);
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    ///     Occurs when [KNX message received].
    /// </summary>
    public event EventHandler<IKnxMessage>? KnxMessageReceived;

    public async Task SendMessageAsync(IKnxMessage knxMessage, CancellationToken cancellationToken = default)
    {
        knxMessage.SourceAddress ??= _deviceAddress;

        var message = (KnxNetIpMessage<TunnelingRequest>)
            KnxNetIpMessage.Create(KnxNetIpServiceType.TunnelingRequest);

        message.Body.Cemi = knxMessage;

        await SendAndReceiveReply<KnxNetIpMessage<TunnelingAcknowledge>>(
            message,
            ack => Task.FromResult(
                ack.Body.CommunicationChannel == message.Body.CommunicationChannel
                && ack.Body.SequenceCounter == message.Body.SequenceCounter),
            cancellationToken);
    }

    public async Task ConnectAsync()
    {
        _udpClient.Connect(_remoteEndPoint);

        var localEndpoint = _udpClient.Client.LocalEndPoint;

        if (localEndpoint == null)
            throw new KnxNetIpException("Unable to retrieve local endpoint.");

        _ = ReceiveMessagesAsync(_receivingMessagesCancellationTokenSource.Token);

        try
        {
            _connectionStateTimeStamp = DateTime.Now;

            if (_udpClient.Client.LocalEndPoint is not IPEndPoint localEndPoint)
                throw new KnxNetIpException("Unable to retrieve local endpoint.");

            var connectRequest = MessageFactory.GetConnectRequest(localEndPoint);
            var connectResponse = await SendAndReceiveReply<KnxNetIpMessage<ConnectionResponse>>(connectRequest);

            if (connectResponse == null)
                throw new KnxNetIpException("Did not retrieve any reply message.");

            if (connectResponse.Body.State != ErrorCode.NoError)
                throw new KnxNetIpException(connectResponse.Body.State);
        }
        catch (KnxException exception)
        {
            throw new KnxException(
                $"Unable to connect to KNX gateway ('{_remoteEndPoint}'): {exception.Message}",
                exception);
        }

        _ = SendKeepAliveMessages(_receivingMessagesCancellationTokenSource.Token);
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            _isDisposed = true;

            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    ~KnxNetIpTunnelingClient()
    {
        Dispose(false);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            _receivingMessagesCancellationTokenSource.Dispose();
            _sendSemaphoreSlim.Dispose();
            _udpClient.Dispose();
        }
    }

    /// <summary>
    ///     Invoked when a KnxMessage has been received
    /// </summary>
    /// <param name="knxMessage"></param>
    private void InvokeKnxMessageReceived(IKnxMessage knxMessage)
    {
        KnxMessageReceived?.Invoke(this, knxMessage);
    }

    private event EventHandler<KnxNetIpMessage>? KnxNetIpMessageReceived;

    private async Task ReceiveMessagesAsync(CancellationToken cancellationToken)
    {
        var receivedBuffer = new List<byte>();

        while (true)
            try
            {
                var udpReceiveResult = await _udpClient.ReceiveAsync(cancellationToken);
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

    private async Task DisconnectAsync()
    {
        try
        {
            if (CommunicationChannel != null && _udpClient.Client.LocalEndPoint is IPEndPoint localEndPoint)
            {
                var disconnectRequest = MessageFactory.GetDisconnectRequest(localEndPoint, (byte)CommunicationChannel);

                await SendAndReceiveReply<KnxNetIpMessage<DisconnectResponse>>(
                    disconnectRequest,
                    ack => Task.FromResult(
                        ack.Body.CommunicationChannel ==
                        ((TunnelingMessageBody)disconnectRequest.Body!).CommunicationChannel));
            }

            _receivingMessagesCancellationTokenSource.Cancel();
        }
        catch (KnxException exception)
        {
            _logger.LogError($"Unable to disconnect from KNX gateway ('{_remoteEndPoint}'): {exception.Message}");

            throw;
        }
    }

    private void OnKnxNetIpMessageReceived(KnxNetIpMessage message)
    {
        _logger.LogInformation("Received message: {message}", message);

        switch (message.ServiceType)
        {
            case KnxNetIpServiceType.ConnectionResponse:
                HandleConnectionResponse((KnxNetIpMessage<ConnectionResponse>)message);

                break;

            case KnxNetIpServiceType.DisconnectResponse:
                HandleDisconnectResponse((KnxNetIpMessage<DisconnectResponse>)message);

                break;

            case KnxNetIpServiceType.ConnectionStateResponse:
                HandleConnectionStateResponse((KnxNetIpMessage<ConnectionStateResponse>)message);

                break;

            case KnxNetIpServiceType.TunnelingRequest:
                HandleTunnelingRequest((KnxNetIpMessage<TunnelingRequest>)message);

                break;
        }

        InvokeKnxNetIpMessageReceived(message);
    }

    private async Task DisposeAsync(bool disposing)
    {
        if (!disposing)
            return;

        await DisconnectAsync();
        _udpClient.Dispose();
    }

    /// <summary>
    ///     Sends the KnxNetIpMessage.
    /// </summary>
    private async Task SendMessageAsync(KnxNetIpMessage message, CancellationToken cancellationToken = default)
    {
        await _sendSemaphoreSlim.WaitAsync(cancellationToken);
        try
        {
            // this should always be true (except for Routing MessageBodies)
            if (message.Body is TunnelingMessageBody tunnelingBody)
            {
                tunnelingBody.CommunicationChannel = CommunicationChannel;
                SetSequenceCount(tunnelingBody);
            }

            await _udpClient.SendAsync(message.ToByteArray(), cancellationToken);

            _logger.LogInformation("Send: {message}", message);
        }
        finally
        {
            _sendSemaphoreSlim.Release();
        }
    }

    private async Task<TResponse> SendAndReceiveReply<TResponse>(KnxNetIpMessage message)
        where TResponse : KnxNetIpMessage
    {
        return await SendAndReceiveReply<TResponse>(
            message,
            _ => Task.FromResult(true));
    }

    /// <summary>
    ///     Checks, if the messageBody requires the sequence counter to be incremented.
    /// </summary>
    /// <param name="knxTunnelingMessageBody">The KNX message body.</param>
    /// <param name="sequenceCounterProperty">The sequence counter property.</param>
    /// <returns>
    ///     <c>true</c>, if the sequenceCounter needs to be incremented; otherwise <c>false</c>
    /// </returns>
    private static bool GetRequiresSequenceCountIncrement(
        TunnelingMessageBody knxTunnelingMessageBody,
        out PropertyInfo? sequenceCounterProperty)
    {
        sequenceCounterProperty = null;

        var bodyType = knxTunnelingMessageBody.GetType();
        var bodyProperties =
            bodyType.GetTypeInfo().DeclaredProperties;

        foreach (var property in bodyProperties)
        {
            var sequenceCounterAttributeArray =
                property.GetCustomAttributes(typeof(SequenceCounterAttribute), true).ToArray();

            if (!sequenceCounterAttributeArray.Any()) continue;

            var sequenceCounterAttribute = (SequenceCounterAttribute)sequenceCounterAttributeArray[0];
            sequenceCounterProperty = property;

            return sequenceCounterAttribute.IncrementOnSendMessage;
        }

        return false;
    }

    private void HandleConnectionResponse(KnxNetIpMessage<ConnectionResponse> knxNetIpMessage)
    {
        if (knxNetIpMessage.Body.State != ErrorCode.NoError)
            return;

        CommunicationChannel = knxNetIpMessage.Body.CommunicationChannel;
        //_keepAliveTimer.Enabled = true;
    }

    private void HandleConnectionStateResponse(KnxNetIpMessage<ConnectionStateResponse> knxNetIpMessage)
    {
        var connectionState = knxNetIpMessage.Body.State;
        if (knxNetIpMessage.Body.State != ErrorCode.NoError)
            _logger.LogError("Connection state is not OK: {connectionState}", connectionState);

        _connectionStateTimeStamp = DateTime.Now;
    }

    private void HandleDisconnectResponse(KnxNetIpMessage<DisconnectResponse> knxNetIpMessage)
    {
        _logger.LogTrace("Received disconnect response: {knxNetIpMessage}", knxNetIpMessage);
    }

    private async void HandleTunnelingRequest(KnxNetIpMessage<TunnelingRequest> knxNetIpMessage)
    {
        await SendAcknowledge(knxNetIpMessage);
        InvokeKnxMessageReceived(knxNetIpMessage.Body.Cemi);
    }

    private async Task SendAcknowledge(KnxNetIpMessage<TunnelingRequest> knxNetIpMessage)
    {
        var acknowledge = KnxNetIpMessage.Create(KnxNetIpServiceType.TunnelingAcknowledge);

        if (acknowledge.Body is not TunnelingAcknowledge acknowledgeBody)
            return;

        acknowledgeBody.CommunicationChannel = knxNetIpMessage.Body.CommunicationChannel;
        acknowledgeBody.State = ErrorCode.NoError;
        acknowledgeBody.SequenceCounter = knxNetIpMessage.Body.SequenceCounter;

        await SendMessageAsync(acknowledge);
    }

    private async Task<TMessage> AwaitMessageAsync<TMessage>(
        Func<TMessage, Task<bool>> condition,
        CancellationToken cancellationToken)
        where TMessage : KnxNetIpMessage
    {
        var messageReceivedCompletionSource = new TaskCompletionSource<TMessage>();

        async void MessageReceived(object? sender, KnxNetIpMessage receivedMessage)
        {
            if (receivedMessage is not TMessage message)
                return;

            if (!await condition.Invoke(message))
                return;

            messageReceivedCompletionSource.TrySetResult(message);
        }

        KnxNetIpMessageReceived += MessageReceived;

        await using var register = cancellationToken.Register(() => messageReceivedCompletionSource.TrySetCanceled());
        try
        {
            return await messageReceivedCompletionSource.Task.ConfigureAwait(false);
        }
        finally
        {
            KnxNetIpMessageReceived -= MessageReceived;
        }
    }

    private async Task<TResponse> SendAndReceiveReply<TResponse>(
        KnxNetIpMessage message,
        Func<TResponse, Task<bool>> condition,
        CancellationToken cancellationToken = default)
        where TResponse : KnxNetIpMessage
    {
        using var timeoutCancellationTokenSource = new CancellationTokenSource(_options.AcknowledgeTimeout);
        using var linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken,
            timeoutCancellationTokenSource.Token);

        var cancellationTokenSource = new CancellationTokenSource(_options.AcknowledgeTimeout);

        var waitTask = AwaitMessageAsync(condition, cancellationTokenSource.Token);
        var sendTask = SendMessageAsync(message, cancellationTokenSource.Token);

        try
        {
            await Task.WhenAll(waitTask, sendTask)
                .ConfigureAwait(false);

            return waitTask.Result;
        }
        catch (Exception ex) when (ex is OperationCanceledException or TaskCanceledException)
        {
            // If the timeout CancellationToken caused the cancellation, throw a NotAcknowledgedException
            if (!cancellationToken.IsCancellationRequested)
            {
                throw new NotAcknowledgedException(
                    $"Message could not be sent and acknowledged within the specified timeout of {_options.AcknowledgeTimeout}");
            }

            throw;
        }
    }

    private async Task SendKeepAliveMessages(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
            try
            {
                _logger.LogInformation("Started sending keep alive messages");

                await Task.Delay(TimeSpan.FromSeconds(59), cancellationToken);

                if (_udpClient.Client.LocalEndPoint is not IPEndPoint localEndPoint)
                    return;

                var connectionStateRequest = MessageFactory.GetConnectionStateRequest(localEndPoint);

                await SendAndReceiveReply<KnxNetIpMessage<ConnectionStateResponse>>(
                    connectionStateRequest,
                    ack => Task.FromResult(
                        ack.Body.CommunicationChannel ==
                        ((TunnelingMessageBody)connectionStateRequest.Body!)
                        .CommunicationChannel),
                    cancellationToken);

                _logger.LogTrace("Send keep alive message");
            }
            catch (Exception ex) when (ex is OperationCanceledException or TaskCanceledException)
            {
                _logger.LogInformation("Stopped sending keep alive messages");

                return;
            }
    }

    /// <summary>
    ///     Sets the sequence count (if required).
    /// </summary>
    /// <param name="knxTunnelingMessageBody">The KNX message body.</param>
    private void SetSequenceCount(TunnelingMessageBody knxTunnelingMessageBody)
    {
        if (GetRequiresSequenceCountIncrement(knxTunnelingMessageBody, out var sequenceProperty))
        {
            sequenceProperty!.SetValue(knxTunnelingMessageBody, _sequenceCounter, null);
            _sequenceCounter++;
        }
    }

    private void InvokeKnxNetIpMessageReceived(KnxNetIpMessage message)
    {
        KnxNetIpMessageReceived?.Invoke(this, message);
    }
}