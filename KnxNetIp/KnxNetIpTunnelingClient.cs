using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Knx.Common;
using Knx.Common.Exceptions;
using Knx.KnxNetIp.MessageBody;
using PortableDI;

namespace Knx.KnxNetIp
{
    /// <summary>
    ///     The KnxClient is used to connect to the knx bus via KnxNetIp protocol.
    /// </summary>
    public class KnxNetIpTunnelingClient : IKnxClient, IDisposable
    {
        #region Static Fields and Constants

        private const int MaxKeepAliveRetries = 3;

        #endregion

        #region Fields and Constants

        private readonly object _communicationChannelLock = new object();
        private readonly object _connectionStateLock = new object();
        private readonly Timer _keepAlive;
        private readonly object _keepAliveLock = new object();
        private readonly KnxNetIpClientMessageListener _messageListener;
        private readonly object _sendLock = new Object();

        private readonly AutoResetEvent _terminationEvent = new AutoResetEvent(false);

        protected IUdpClient InternalUdpClient;

        private byte? _currentCommunicationChannel;
        private bool _isClosing;
        private bool _isDisposed;
        private int _keepAliveRetry;
        private bool _logicalConnected;
        private byte _sequenceCounter;

        #endregion

        #region Constructor / Destructor

        public KnxNetIpTunnelingClient(IPEndPoint remoteEndPoint, KnxDeviceAddress deviceAddress)
        {
            // default Values
            ReadTimeout = TimeSpan.FromSeconds(60);
            SendMessageTimeout = TimeSpan.FromSeconds(15);

            ConnectionStateTimeStamp = DateTime.MinValue;
            DeviceAddress = deviceAddress;

            RemoteEndPoint = remoteEndPoint ?? throw new ArgumentNullException("remoteEndPoint");

            InternalUdpClient = DIContainer.Resolve<IUdpClient>();
            _messageListener = new KnxNetIpClientMessageListener(InternalUdpClient);

            _keepAlive = new Timer(SendKeepAliveMessage, null, Timeout.Infinite, Timeout.Infinite);
        }

        #endregion

        #region Properties

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
        ///     Gets a value indicating whether this instance has keep alive timeouted.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has keep alive timeouted; otherwise, <c>false</c>.
        /// </value>
        public bool HasKeepAliveTimeout
        {
            get { return (ConnectionStateTimeStamp < DateTime.Now.Subtract(TimeSpan.FromSeconds(61))); }
        }

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
                    return InternalUdpClient.LocalEndpoint;
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

        #endregion

        #region IKnxClient Members

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
            get { return _logicalConnected && (HasKeepAliveTimeout == false) && (ConnectionState == ErrorCode.NoError); }
        }

        public void Open()
        {
            lock (_connectionStateLock)
            {
                StartInternalCommunicationCentral();

                var connectRequest = MessageFactory.GetConnectRequest(LocalEndPoint);

                try
                {
                    ConnectionStateTimeStamp = DateTime.Now;

                    KnxNetIpMessage<ConnectionResponse> connectResponse;
                    SendAndReceiveReply(connectRequest, out connectResponse);

                    if (connectResponse == null)
                    {
                        throw new KnxNetIpException("Did not retrieve any reply message.");
                    }

                    if (connectResponse.Body.State != ErrorCode.NoError)
                    {
                        throw new KnxNetIpException(connectResponse.Body.State);
                    }

                    _logicalConnected = true;
                }
                catch (KnxException exception)
                {
                    throw new KnxException(string.Format("Unable to connect to KNX gateway ('{0}'): {1}", RemoteEndPoint, exception.Message), exception);
                }
                catch (Exception)
                {
                    ShutdownInternalCommunicationCentral();
                    throw;
                }
                finally
                {
                    var result = IsConnected;
                }
            }
        }

        public void SendMessage(IKnxMessage knxMessage)
        {
            var message = (KnxNetIpMessage<TunnelingRequest>) KnxNetIpMessage.Create(KnxNetIpServiceType.TunnelingRequest);

            message.Body.Cemi = knxMessage;

            SendAndReceiveReply(message, out KnxNetIpMessage<TunnelingAcknowledge> _,
                (ack) => (ack.Body.CommunicationChannel == message.Body.CommunicationChannel)
                         && (ack.Body.SequenceCounter == message.Body.SequenceCounter));
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if ((disposing) && (!_isDisposed))
            {
                _terminationEvent.Set();

                Disconnect();

                _keepAlive.Dispose();
                _messageListener.Dispose();
                _terminationEvent.Dispose();

                _isDisposed = true;
            }

            // free unmanaged resources if there are any
        }

        protected virtual void NetIpMessageListenerKnxNetIpMessageReceived(object sender, KnxNetIpMessage netIpMessage)
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
        protected void Send(KnxNetIpMessage netIpMessage)
        {
            lock (_sendLock)
            {
                // this should always be true (except for Routing MessageBodies)
                if (netIpMessage.Body is TunnelingMessageBody tunnelingBody)
                {
                    tunnelingBody.CommunicationChannel = CommunicationChannel;
                    SetSequenceCount(tunnelingBody);
                }

                Debug.WriteLine(string.Format("{0} SEND => {1}", DateTime.Now.ToLongTimeString(), netIpMessage));

                var messageBytes = netIpMessage.ToByteArray();
                InternalUdpClient.Send(messageBytes, messageBytes.Length);
            }
        }

        protected void SendAndReceiveReply<TResponse>(KnxNetIpMessage message, out TResponse response) where TResponse : KnxNetIpMessage
        {
            SendAndReceiveReply(message, out response, (ack) => !ack.Equals(default(KnxNetIpMessage)));
        }

        protected void ShutdownInternalCommunicationCentral()
        {
            _messageListener.KnxNetIpMessageReceived -= NetIpMessageListenerKnxNetIpMessageReceived;
            _messageListener.Stop();
            InternalUdpClient.Close();
        }

        protected void StartInternalCommunicationCentral()
        {
            InternalUdpClient.Connect(RemoteEndPoint);
            _messageListener.KnxNetIpMessageReceived += NetIpMessageListenerKnxNetIpMessageReceived;
            _messageListener.Start();
        }

        /// <summary>
        ///     Starts the keep alive.
        /// </summary>
        protected void StartKeepAlive(bool immediately = false)
        {
            lock (_keepAliveLock)
            {
                _keepAlive.Change(TimeSpan.FromSeconds(immediately ? 0 : 59), TimeSpan.FromSeconds(59));
            }
        }

        /// <summary>
        ///     Stops the keep alive.
        /// </summary>
        protected void StopKeepAlive()
        {
            lock (_keepAliveLock)
            {
                ConnectionStateTimeStamp = DateTime.MinValue;
                if (_keepAlive != null) _keepAlive.Change(Timeout.Infinite, Timeout.Infinite);
            }
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

        private void Disconnect()
        {
            lock (_connectionStateLock)
            {
                _isClosing = true;
                try
                {
                    StopKeepAlive();

                    if (CommunicationChannel != null)
                    {
                        var disconnectRequest = MessageFactory.GetDisconnectRequest(LocalEndPoint, (byte) CommunicationChannel);
                        KnxNetIpMessage<DisconnectResponse> disconnectResponse;
                        SendAndReceiveReply(disconnectRequest, out disconnectResponse, (ack) => ack.Body.CommunicationChannel == ((TunnelingMessageBody) disconnectRequest.Body).CommunicationChannel);
                    }
                }
                catch (KnxException exception)
                {
                    Debug.WriteLine(string.Format("Unable to disconnect from KNX gateway ('{0}'): {1}", RemoteEndPoint, exception.Message));
                }
                finally
                {
                    _logicalConnected = false;
                    ShutdownInternalCommunicationCentral();
                    _isClosing = false;
                }
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

        private void HandleTunnelingRequest(KnxNetIpMessage<TunnelingRequest> knxNetIpMessage)
        {
            SendAcknowledge(knxNetIpMessage);
            InvokeKnxMessageReceived(knxNetIpMessage.Body.Cemi);
        }

        private void InvokeKnxMessageReceived(IKnxMessage cemi)
        {
            if (KnxMessageReceived != null)
            {
                KnxMessageReceived(this, cemi);
            }
        }

        private void SendAcknowledge(KnxNetIpMessage<TunnelingRequest> knxNetIpMessage)
        {
            var acknowledge = KnxNetIpMessage.Create(KnxNetIpServiceType.TunnelingAcknowledge);
            var acknowledgeBody = acknowledge.Body as TunnelingAcknowledge;

            if (acknowledgeBody == null)
            {
                return;
            }

            acknowledgeBody.CommunicationChannel = knxNetIpMessage.Body.CommunicationChannel;
            acknowledgeBody.State = ErrorCode.NoError;
            acknowledgeBody.SequenceCounter = knxNetIpMessage.Body.SequenceCounter;
            Send(acknowledge);
        }

        /// <summary>
        /// TODO: Make a real async call 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="response"></param>
        /// <param name="condition"></param>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="NotAcknowledgedException"></exception>
        private void SendAndReceiveReply<TResponse>(KnxNetIpMessage message, out TResponse response, Func<TResponse, bool> condition) where TResponse : KnxNetIpMessage
        {
            if (_messageListener == null)
                throw new NullReferenceException("_messageListener");

            using (var waitForReplyEvent = new AutoResetEvent(false))
            {
                var waitHandles = new WaitHandle[] {waitForReplyEvent, _terminationEvent};
                var receivedResponseMessage = default(TResponse);

                KnxNetIpMessageReceivedHandler knxNetIpMessageReceived = (sender, receivedMessage) =>
                {
                    if (receivedMessage is TResponse)
                    {
                        if (condition.Invoke((TResponse) receivedMessage))
                        {
                            receivedResponseMessage = (TResponse) receivedMessage;
                            waitForReplyEvent.Set();

                            Debug.WriteLine(string.Format("{0} RECV <= {1} (HANDLED)", DateTime.Now.ToLongTimeString(), receivedMessage.ToString()));
                        }
                    }
                };

                _messageListener.KnxNetIpMessageReceived += knxNetIpMessageReceived;
                try
                {
                    Send(message);

                    if (SendMessageTimeout.TotalSeconds < 3)
                        SendMessageTimeout = TimeSpan.FromSeconds(3);

                    switch (WaitHandle.WaitAny(waitHandles, SendMessageTimeout))
                    {
                        case WaitHandle.WaitTimeout:
                            throw new NotAcknowledgedException(string.Format("Expected response was not retrieved during specified timeout of {0}. Message: {1}", SendMessageTimeout, message));
                        case 0: // reply event
                            response = receivedResponseMessage;
                            break;
                        case 1: // termination event
                            response = null;
                            break;
                        default: // should never happen
                            Debug.WriteLine("KnxNetIpTunnelingClient.SendAndReceiveReply: 'Should Never Happen' HAPPEND!!");
                            response = null;
                            break;
                    }
                }
                finally
                {
                    // maybe the driver is closed during waiting for a message.
                    if (_messageListener != null)
                    {
                        _messageListener.KnxNetIpMessageReceived -= knxNetIpMessageReceived;
                    }
                }
            }
        }

        /// <summary>
        ///     Send a keep alive message.
        /// </summary>
        private void SendKeepAliveMessage(object state)
        {
            if (_isClosing)
            {
                return;
            }

            lock (_keepAliveLock)
            {
                var connectionStateRequest = MessageFactory.GetConnectionStateRequest(LocalEndPoint);

                try
                {
                    SendAndReceiveReply(connectionStateRequest, out KnxNetIpMessage<ConnectionStateResponse> _, (ack) => ack.Body.CommunicationChannel == ((TunnelingMessageBody) connectionStateRequest.Body).CommunicationChannel);
                    _keepAliveRetry = 0;
                }
                catch (Exception)
                {
                    _keepAliveRetry++;

                    if (_keepAliveRetry < MaxKeepAliveRetries)
                        SendKeepAliveMessage(state);
                }
            }
        }

        /// <summary>
        ///     Sets the sequence count (if required).
        /// </summary>
        /// <param name="knxTunnelingMessageBody">The KNX message body.</param>
        private void SetSequenceCount(TunnelingMessageBody knxTunnelingMessageBody)
        {
            PropertyInfo sequenceProperty;

            if (DoesRequireSequenceCountIncrement(knxTunnelingMessageBody, out sequenceProperty))
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