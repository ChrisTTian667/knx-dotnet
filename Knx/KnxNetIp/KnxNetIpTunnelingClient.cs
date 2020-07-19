using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Knx.Common;
using Knx.Common.Exceptions;
using Knx.KnxNetIp.MessageBody;

namespace Knx.KnxNetIp
{
    /// <summary>
    ///     The KnxClient is used to connect to the knx bus via KnxNetIp protocol.
    /// </summary>
    public sealed class KnxNetIpTunnelingClient : IKnxClient
    {
        private readonly SemaphoreSlim _sendSemaphoreSlim = new SemaphoreSlim(1,1);
        private readonly object _communicationChannelLock = new object();
        private readonly object _connectionStateLock = new object();
        //private readonly Timer _keepAlive;
        private readonly object _keepAliveLock = new object();

        private const int MaxKeepAliveRetries = 3;
        
        private KnxNetIpClientMessageListener _messageListener;    // TODO: Refactor this, because its only allowed to exist once per client.
        private readonly object _sendLock = new object();

        private readonly AutoResetEvent _terminationEvent = new AutoResetEvent(false);

        private byte? _currentCommunicationChannel;
        private bool _isClosing;
        private bool _isDisposed;
        private int _keepAliveRetry;
        private bool _logicalConnected;
        private byte _sequenceCounter;
        private readonly UdpClient _udpClient;

        public KnxNetIpTunnelingClient(IPEndPoint remoteEndPoint, KnxDeviceAddress deviceAddress)
        {
            // default Values
            ReadTimeout = TimeSpan.FromSeconds(60);
            SendMessageTimeout = TimeSpan.FromSeconds(15);

            ConnectionStateTimeStamp = DateTime.MinValue;
            DeviceAddress = deviceAddress;

            RemoteEndPoint = remoteEndPoint ?? throw new ArgumentNullException(nameof(remoteEndPoint));

            _udpClient = new UdpClient();
            
            // TODO: reenable KeepAlive, but use a better threading approach 
            // _keepAlive = new Timer(async (state) => await SendKeepAliveMessage(), null, Timeout.Infinite, Timeout.Infinite);
        }

        public TimeSpan SendMessageTimeout { get; set; }

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

        public KnxDeviceAddress DeviceAddress { get; private set; }

        public TimeSpan ReadTimeout { get; set; }

        public bool IsConnected
        {
            get { return _logicalConnected && (IsTimedOut == false) && (ConnectionState == ErrorCode.NoError); }
        }

        public async Task Open()
        {
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
                ShutdownInternalCommunicationCentral();
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
            if ((disposing) && (!_isDisposed))
            {
                _terminationEvent.Set();
                Disconnect();
                _messageListener.Dispose();
                _terminationEvent.Dispose();
                _isDisposed = true;
            }

            // free unmanaged resources if there are any
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

        private void ShutdownInternalCommunicationCentral()
        {
            _messageListener.KnxNetIpMessageReceived -= NetIpMessageListenerKnxNetIpMessageReceived;
            _messageListener.Dispose();
        }

        /// <summary>
        ///     Starts the keep alive.
        /// </summary>
        private void StartKeepAlive()
        {

            // lock (_keepAliveLock)
            // {
            //     _keepAlive.Change(TimeSpan.FromSeconds(immediately ? 0 : 59), TimeSpan.FromSeconds(59));
            // }
        }

        /// <summary>
        ///     Stops the keep alive.
        /// </summary>
        private void StopKeepAlive()
        {
            // lock (_keepAliveLock)
            // {
            //     _keepAlive?.Change(Timeout.Infinite, Timeout.Infinite);
            // }
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

        private async Task Disconnect()
        {
                _isClosing = true;
                try
                {
                    StopKeepAlive();

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
                    _logicalConnected = false;
                    ShutdownInternalCommunicationCentral();
                    _isClosing = false;
                }
        }

        private void HandleConnectionResponse(KnxNetIpMessage<ConnectionResponse> knxNetIpMessage)
        {
            if (knxNetIpMessage.Body.State == ErrorCode.NoError)
            {
                CommunicationChannel = knxNetIpMessage.Body.CommunicationChannel;
                StartKeepAlive();
            }
        }

        private void HandleConnectionStateResponse(KnxNetIpMessage<ConnectionStateResponse> knxNetIpMessage)
        {
            ConnectionState = knxNetIpMessage.Body.State;
            ConnectionStateTimeStamp = DateTime.Now;
        }

        private void HandleDisconnectResponse(KnxNetIpMessage<DisconnectResponse> knxNetIpMessage)
        {
            StopKeepAlive();
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

        /// <summary>
        /// TODO: Make a real async call 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="condition"></param>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="NotAcknowledgedException"></exception>
        private async Task<TResponse> SendAndReceiveReply<TResponse>(KnxNetIpMessage message, Func<TResponse, bool> condition) where TResponse : KnxNetIpMessage
        {
            if (_messageListener == null)
                throw new NullReferenceException("_messageListener");

            using (var waitForReplyEvent = new AutoResetEvent(false))
            {
                var waitHandles = new WaitHandle[] {waitForReplyEvent, _terminationEvent};
                var receivedResponseMessage = default(TResponse);

                KnxNetIpMessageReceivedHandler knxNetIpMessageReceived = (sender, receivedMessage) =>
                {
                    if (receivedMessage is TResponse response && condition.Invoke(response))
                    {
                        receivedResponseMessage = response;
                        waitForReplyEvent.Set();

                        Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} RECV <= {response.ToString()} (HANDLED)");
                    }
                };

                _messageListener.KnxNetIpMessageReceived += knxNetIpMessageReceived;
                try
                {
                    await Send(message);

                    if (SendMessageTimeout.TotalSeconds < 3)
                        SendMessageTimeout = TimeSpan.FromSeconds(3);

                    switch (WaitHandle.WaitAny(waitHandles, SendMessageTimeout))
                    {
                        case WaitHandle.WaitTimeout:
                            throw new NotAcknowledgedException(string.Format("Expected response was not retrieved during specified timeout of {0}. Message: {1}", SendMessageTimeout, message));
                        case 0: // reply event
                            return receivedResponseMessage;
                        default:
                            return null;
                    }
                }
                finally
                {
                    if (_messageListener != null)
                        _messageListener.KnxNetIpMessageReceived -= knxNetIpMessageReceived;
                }
            }
        }

        /// <summary>
        ///     Send a keep alive message.
        /// </summary>
        private async Task SendKeepAliveMessage()
        {
            if (_isClosing)
            {
                return;
            }

            var connectionStateRequest = MessageFactory.GetConnectionStateRequest(LocalEndPoint);

            try
            {
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