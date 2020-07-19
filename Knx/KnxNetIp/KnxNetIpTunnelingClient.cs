using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Knx.Exceptions;
using Knx.KnxNetIp.MessageBody;

namespace Knx.KnxNetIp
{
    /// <summary>
    /// Used to connect to the Knx Bus via KnxNetIpTunneling protocol.
    /// </summary>
    public sealed class KnxNetIpTunnelingClient : IKnxClient
    {
        private readonly SemaphoreSlim _sendSemaphoreSlim = new SemaphoreSlim(1,1);
        private readonly object _communicationChannelLock = new object();
        private readonly System.Timers.Timer _keepAliveTimer;
        
        private const int MaxKeepAliveRetries = 3;
        
        private KnxNetIpClientMessageListener _messageListener;    // TODO: Refactor this, because its only allowed to exist once per client.
        private readonly AutoResetEvent _terminationEvent = new AutoResetEvent(false);

        private byte? _currentCommunicationChannel;
        private int _keepAliveRetry;
        private bool _logicalConnected;
        private byte _sequenceCounter;
        private readonly UdpClient _udpClient;

        // TODO: Add a configuration object for easy specification of the settings
        public KnxNetIpTunnelingClient(IPEndPoint remoteEndPoint, KnxDeviceAddress deviceAddress)
        {
            // default Values
            ReadTimeout = TimeSpan.FromSeconds(60);
            SendMessageTimeout = TimeSpan.FromSeconds(15);

            ConnectionStateTimeStamp = DateTime.MinValue;
            DeviceAddress = deviceAddress;

            RemoteEndPoint = remoteEndPoint ?? throw new ArgumentNullException(nameof(remoteEndPoint));

            _udpClient = new UdpClient();

            _keepAliveTimer = new System.Timers.Timer(59000) {Enabled = false, AutoReset = true};
            _keepAliveTimer.Elapsed += async (sender, args) => await SendKeepAliveMessage();
        }

        public TimeSpan ReadTimeout { get; private set; }
        public TimeSpan SendMessageTimeout { get; private set; }

        /// <summary>
        ///     Gets or sets the current communication channel.
        /// </summary>
        /// <value>The communication channel.</value>
        public byte? CommunicationChannel
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
        ///     Gets or sets the state of the connection.
        /// </summary>
        /// <value>The state of the connection.</value>
        public ErrorCode ConnectionState { get; private set; }

        /// <summary>
        ///     Gets or sets the connection state time stamp.
        /// </summary>
        /// <value>The connection state time stamp.</value>
        public DateTime ConnectionStateTimeStamp { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether this instance timed out.
        /// </summary>
        public bool IsTimedOut => (ConnectionStateTimeStamp < DateTime.Now.Subtract(TimeSpan.FromSeconds(61)));

        /// <summary>
        ///     Gets the local IPEndPoint.
        /// </summary>
        /// <value>The local IPEndPoint.</value>
        public IPEndPoint LocalEndPoint
        {
            get
            {
                try
                {
                    return _udpClient.Client.LocalEndPoint as IPEndPoint;
                }
                catch (Exception)
                {
                    throw new Exception("Client not connected.");
                }
            }
        }

        /// <summary>
        ///     Gets the remote IPEndPoint.
        /// </summary>
        /// <value>The remote IPEndPoint.</value>
        public IPEndPoint RemoteEndPoint { get; private set; }

        public event KnxMessageReceivedHandler KnxMessageReceived;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public KnxDeviceAddress DeviceAddress { get; }



        public bool IsConnected => _logicalConnected && (IsTimedOut == false) && (ConnectionState == ErrorCode.NoError);

        public async Task Connect()
        {
            if (_messageListener != null)
                throw new KnxNetIpException("Client is already connected.");
            
            _udpClient.Connect(RemoteEndPoint);
            
            _messageListener = new KnxNetIpClientMessageListener(_udpClient);
            _messageListener.KnxNetIpMessageReceived += NetIpMessageListenerKnxNetIpMessageReceived;
                
            try
            {
                ConnectionStateTimeStamp = DateTime.Now;

                var connectRequest = MessageFactory.GetConnectRequest(LocalEndPoint);
                var connectResponse = await SendAndReceiveReply<KnxNetIpMessage<ConnectionResponse>>(connectRequest);
                if (connectResponse == null)
                    throw new KnxNetIpException("Did not retrieve any reply message.");
                if (connectResponse.Body.State != ErrorCode.NoError)
                    throw new KnxNetIpException(connectResponse.Body.State);

                _logicalConnected = true;
            }
            catch (KnxException exception)
            {
                throw new KnxException($"Unable to connect to KNX gateway ('{RemoteEndPoint}'): {exception.Message}", exception);
            }
            catch (Exception)
            {
                _messageListener.KnxNetIpMessageReceived -= NetIpMessageListenerKnxNetIpMessageReceived;
                _messageListener.Dispose();
                _messageListener = null;
                
                throw;
            }
        }

        public async Task SendMessage(IKnxMessage knxMessage)
        {
            var message = (KnxNetIpMessage<TunnelingRequest>) KnxNetIpMessage.Create(KnxNetIpServiceType.TunnelingRequest);

            message.Body.Cemi = knxMessage;

            await SendAndReceiveReply<KnxNetIpMessage<TunnelingAcknowledge>>(message,
                (ack) => (ack.Body.CommunicationChannel == message.Body.CommunicationChannel)
                         && (ack.Body.SequenceCounter == message.Body.SequenceCounter));
        }

        private void Dispose(bool disposing)
        {
            if (!disposing) 
                return;
            
            _terminationEvent.Set();
            Disconnect().Wait();
            _terminationEvent?.Dispose();
        }

        private void NetIpMessageListenerKnxNetIpMessageReceived(object sender, KnxNetIpMessage netIpMessage)
        {
            Debug.WriteLine("{0} RECV <= {1}", DateTime.Now.ToLongTimeString(), netIpMessage);

            switch (netIpMessage.ServiceType)
            {
                case KnxNetIpServiceType.ConnectionResponse:
                    HandleConnectionResponse(netIpMessage as KnxNetIpMessage<ConnectionResponse>);
                    break;

                case KnxNetIpServiceType.DisconnectResponse:
                    HandleDisconnectResponse(netIpMessage as KnxNetIpMessage<DisconnectResponse>);
                    break;

                case KnxNetIpServiceType.ConnectionStateResponse:
                    HandleConnectionStateResponse(netIpMessage as KnxNetIpMessage<ConnectionStateResponse>);
                    break;

                case KnxNetIpServiceType.TunnelingRequest:
                    HandleTunnelingRequest(netIpMessage as KnxNetIpMessage<TunnelingRequest>);
                    break;
            }
        }
        
        /// <summary>
        ///     Sends the KnxNetIpMessage.
        /// </summary>
        /// <param name="netIpMessage">The message.</param>
        private async Task Send(KnxNetIpMessage netIpMessage)
        {
            await _sendSemaphoreSlim.WaitAsync();

            try
            {
                // this should always be true (except for Routing MessageBodies)
                if (netIpMessage.Body is TunnelingMessageBody tunnelingBody)
                {
                    tunnelingBody.CommunicationChannel = CommunicationChannel;
                    SetSequenceCount(tunnelingBody);
                }

                Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} SEND => {netIpMessage}");

                var messageBytes = netIpMessage.ToByteArray();
                await _udpClient.SendAsync(messageBytes, messageBytes.Length);
            }
            finally
            {
                _sendSemaphoreSlim.Release();
            }
        }

        private async Task<TResponse> SendAndReceiveReply<TResponse>(KnxNetIpMessage message) where TResponse : KnxNetIpMessage
        {
            return await SendAndReceiveReply<TResponse>(message, (ack) => !ack.Equals(default(KnxNetIpMessage)));
        }

        /// <summary>
        ///     Checks, if the messageBody requires the sequence counter to be incremented.
        /// </summary>
        /// <param name="knxTunnelingMessageBody">The KNX message body.</param>
        /// <param name="sequenceCounterProperty">The sequence counter property.</param>
        /// <returns>
        ///     <c>true</c>, if the sequenceCounter needs to be incremented; otherwise <c>false</c>
        /// </returns>
        private static bool DoesRequireSequenceCountIncrement(TunnelingMessageBody knxTunnelingMessageBody, out PropertyInfo sequenceCounterProperty)
        {
            sequenceCounterProperty = null;

            var bodyType = knxTunnelingMessageBody.GetType();
            var bodyProperties =
                bodyType.GetTypeInfo().DeclaredProperties;

            foreach (var property in bodyProperties)
            {
                var sequnceCounterAttributeArray = property.GetCustomAttributes(typeof (SequenceCounterAttribute), true).ToArray();
                if (!sequnceCounterAttributeArray.Any())
                {
                    continue;
                }

                var sequenceCounterAttribute = (SequenceCounterAttribute) (sequnceCounterAttributeArray[0]);
                sequenceCounterProperty = property;

                return sequenceCounterAttribute.IncrementOnSendMessage;
            }

            return false;
        }

        public async Task Disconnect()
        {
            try
            {
                _keepAliveTimer.Enabled = false;
                
                if (CommunicationChannel != null)
                {
                    var disconnectRequest = MessageFactory.GetDisconnectRequest(LocalEndPoint, (byte) CommunicationChannel);
                    await SendAndReceiveReply<KnxNetIpMessage<DisconnectResponse>>(disconnectRequest, (ack) => ack.Body.CommunicationChannel == ((TunnelingMessageBody) disconnectRequest.Body).CommunicationChannel);
                }
            }
            catch (KnxException exception)
            {
                Debug.WriteLine($"Unable to disconnect from KNX gateway ('{RemoteEndPoint}'): {exception.Message}");
            }
            finally
            {
                _messageListener.KnxNetIpMessageReceived -= NetIpMessageListenerKnxNetIpMessageReceived;
                _messageListener.Dispose();
                _messageListener = null;
                _logicalConnected = false;
            }
        }

        private void HandleConnectionResponse(KnxNetIpMessage<ConnectionResponse> knxNetIpMessage)
        {
            if (knxNetIpMessage.Body.State != ErrorCode.NoError) 
                return;
            
            CommunicationChannel = knxNetIpMessage.Body.CommunicationChannel;
            _keepAliveTimer.Enabled = true;
        }

        private void HandleConnectionStateResponse(KnxNetIpMessage<ConnectionStateResponse> knxNetIpMessage)
        {
            ConnectionState = knxNetIpMessage.Body.State;
            ConnectionStateTimeStamp = DateTime.Now;
        }

        private void HandleDisconnectResponse(KnxNetIpMessage<DisconnectResponse> knxNetIpMessage)
        {
            if (knxNetIpMessage != null)            
                _keepAliveTimer.Enabled = false;
        }

        private async void HandleTunnelingRequest(KnxNetIpMessage<TunnelingRequest> knxNetIpMessage)
        {
            await SendAcknowledge(knxNetIpMessage);
            InvokeKnxMessageReceived(knxNetIpMessage.Body.Cemi);
        }

        private void InvokeKnxMessageReceived(IKnxMessage cemi)
        {
            KnxMessageReceived?.Invoke(this, cemi);
        }

        private async Task SendAcknowledge(KnxNetIpMessage<TunnelingRequest> knxNetIpMessage)
        {
            var acknowledge = KnxNetIpMessage.Create(KnxNetIpServiceType.TunnelingAcknowledge);
            if (!(acknowledge.Body is TunnelingAcknowledge acknowledgeBody))
                return;

            acknowledgeBody.CommunicationChannel = knxNetIpMessage.Body.CommunicationChannel;
            acknowledgeBody.State = ErrorCode.NoError;
            acknowledgeBody.SequenceCounter = knxNetIpMessage.Body.SequenceCounter;
            await Send(acknowledge);
        }

        private async Task<TResponse> SendAndReceiveReply<TResponse>(KnxNetIpMessage message, Func<TResponse, bool> condition) where TResponse : KnxNetIpMessage
        {
            if (_messageListener == null)
                throw new InvalidOperationException("There is no message listener.");

            var responseCompletionSource = new TaskCompletionSource<TResponse>();
           
            KnxNetIpMessageReceivedHandler knxNetIpMessageReceived = (sender, receivedMessage) =>
            {
                if (!(receivedMessage is TResponse response) || !condition.Invoke(response)) 
                    return;

                responseCompletionSource.TrySetResult(response);
                Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} RECV <= {response} (HANDLED)");
            };

            _messageListener.KnxNetIpMessageReceived += knxNetIpMessageReceived;
            try
            {
                // send the message
                var sendMessageTask = Task.Run(() => Send(message));
                if (!sendMessageTask.Wait(SendMessageTimeout))
                    throw new NotAcknowledgedException($"Message could not be send within specified timeout of {SendMessageTimeout}. Message: {message}");

                // receive the response
                var receiveMessageTask = Task.Run(async () => await responseCompletionSource.Task);
                if (!receiveMessageTask.Wait(ReadTimeout))
                    throw new NotAcknowledgedException($"Expected response was not retrieved during specified timeout of {SendMessageTimeout}. Message: {message}");

                return await responseCompletionSource.Task;
            }
            finally
            {
                if (_messageListener != null)
                    _messageListener.KnxNetIpMessageReceived -= knxNetIpMessageReceived;
            }
        }

        /// <summary>
        ///     Send a keep alive message.
        /// </summary>
        private async Task SendKeepAliveMessage()
        {
            try
            {
                var connectionStateRequest = MessageFactory.GetConnectionStateRequest(LocalEndPoint);
                await SendAndReceiveReply<KnxNetIpMessage<ConnectionStateResponse>>(connectionStateRequest, (ack) => ack.Body.CommunicationChannel == ((TunnelingMessageBody) connectionStateRequest.Body).CommunicationChannel);
                _keepAliveRetry = 0;
            }
            catch (Exception)
            {
                _keepAliveRetry++;

                if (_keepAliveRetry < MaxKeepAliveRetries)
                    await SendKeepAliveMessage();
            }
        }

        /// <summary>
        ///     Sets the sequence count (if required).
        /// </summary>
        /// <param name="knxTunnelingMessageBody">The KNX message body.</param>
        private void SetSequenceCount(TunnelingMessageBody knxTunnelingMessageBody)
        {
            if (DoesRequireSequenceCountIncrement(knxTunnelingMessageBody, out PropertyInfo sequenceProperty))
            {
                sequenceProperty.SetValue(knxTunnelingMessageBody, _sequenceCounter, null);
                _sequenceCounter++;
            }
        }

        ~KnxNetIpTunnelingClient()
        {
            Dispose(false);
        }
    }
}