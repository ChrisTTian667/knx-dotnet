﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
    public sealed class KnxNetIpTunnelingClient : KnxNetIpClient
    {
        private readonly SemaphoreSlim _sendSemaphoreSlim = new SemaphoreSlim(1,1);
        private readonly object _communicationChannelLock = new object();
        private readonly System.Timers.Timer _keepAliveTimer;
        private readonly AutoResetEvent _terminationEvent = new AutoResetEvent(false);
        private byte? _currentCommunicationChannel;
        private bool _logicalConnected;
        private byte _sequenceCounter;

        private event EventHandler<KnxNetIpMessage> KnxNetIpMessageReceived;
        
        public KnxNetIpTunnelingClient(IPEndPoint remoteEndPoint, KnxDeviceAddress deviceAddress, KnxNetIpConfiguration configuration = null) : base(remoteEndPoint, deviceAddress, configuration)
        {
            ConnectionStateTimeStamp = DateTime.MinValue;

            _keepAliveTimer = new System.Timers.Timer(59000) {Enabled = false, AutoReset = true};
            _keepAliveTimer.Elapsed += async (sender, args) => await SendKeepAliveMessage();
        }

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
                    return UdpClient.Client.LocalEndPoint as IPEndPoint;
                }
                catch (Exception)
                {
                    throw new Exception("Client not connected.");
                }
            }
        }
        
        public override bool IsConnected
        {
            get => _logicalConnected && IsTimedOut == false && (ConnectionState == ErrorCode.NoError);
            protected set => throw new InvalidOperationException($"Don't set {nameof(IsConnected)}!");
        }

        public override async Task Connect()
        {
            await base.Connect();
            
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
        }
        
        public override async Task Disconnect()
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
                _logicalConnected = false;
            }
        }

        public override async Task SendMessage(IKnxMessage knxMessage)
        {
            var message = (KnxNetIpMessage<TunnelingRequest>) KnxNetIpMessage.Create(KnxNetIpServiceType.TunnelingRequest);

            message.Body.Cemi = knxMessage;

            await SendAndReceiveReply<KnxNetIpMessage<TunnelingAcknowledge>>(message,
                (ack) => (ack.Body.CommunicationChannel == message.Body.CommunicationChannel)
                         && (ack.Body.SequenceCounter == message.Body.SequenceCounter));
        }

        protected override void OnKnxNetIpMessageReceived(KnxNetIpMessage message)
        {
            Debug.WriteLine("{0} RECV <= {1}", DateTime.Now.ToLongTimeString(), message);

            switch (message.ServiceType)
            {
                case KnxNetIpServiceType.ConnectionResponse:
                    HandleConnectionResponse(message as KnxNetIpMessage<ConnectionResponse>);
                    break;

                case KnxNetIpServiceType.DisconnectResponse:
                    HandleDisconnectResponse(message as KnxNetIpMessage<DisconnectResponse>);
                    break;

                case KnxNetIpServiceType.ConnectionStateResponse:
                    HandleConnectionStateResponse(message as KnxNetIpMessage<ConnectionStateResponse>);
                    break;

                case KnxNetIpServiceType.TunnelingRequest:
                    HandleTunnelingRequest(message as KnxNetIpMessage<TunnelingRequest>);
                    break;
            }

            InvokeKnxNetIpMessageReceived(message);
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing) 
                return;
            
            _terminationEvent.Set();
            Disconnect().Wait();
            _terminationEvent?.Dispose();
            
            base.Dispose(true);
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
                await UdpClient.SendAsync(messageBytes, messageBytes.Length);
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
            var responseCompletionSource = new TaskCompletionSource<TResponse>();

            void NetIpMessageReceived(object sender, KnxNetIpMessage receivedMessage)
            {
                if (!(receivedMessage is TResponse response) || !condition.Invoke(response)) return;

                responseCompletionSource.TrySetResult(response);
                Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} RECV <= {response} (HANDLED)");
            }

            KnxNetIpMessageReceived += NetIpMessageReceived;
            try
            {
                // send the message
                var sendMessageTask = Task.Run(() => Send(message));
                if (!sendMessageTask.Wait(Configuration.SendMessageTimeout))
                    throw new NotAcknowledgedException($"Message could not be send within specified timeout of {Configuration.SendMessageTimeout}. Message: {message}");

                // receive the response
                var receiveMessageTask = Task.Run(async () => await responseCompletionSource.Task);
                if (!receiveMessageTask.Wait(Configuration.ReadTimeout))
                    throw new NotAcknowledgedException($"Expected response was not retrieved during specified timeout of {Configuration.SendMessageTimeout}. Message: {message}");

                return await responseCompletionSource.Task;
            }
            finally
            {
                KnxNetIpMessageReceived -= NetIpMessageReceived;
            }
        }

        /// <summary>
        ///     Send a keep alive message.
        /// </summary>
        private async Task SendKeepAliveMessage(int retry = 0)
        {
            try
            {
                var connectionStateRequest = MessageFactory.GetConnectionStateRequest(LocalEndPoint);
                await SendAndReceiveReply<KnxNetIpMessage<ConnectionStateResponse>>(connectionStateRequest, (ack) => ack.Body.CommunicationChannel == ((TunnelingMessageBody) connectionStateRequest.Body).CommunicationChannel);
            }
            catch (Exception)
            {
                retry +=1;

                if (retry < Configuration.MaxKeepAliveRetries)
                {
                    await Task.Delay(1000);
                    await SendKeepAliveMessage(retry);
                }
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

        private void InvokeKnxNetIpMessageReceived(KnxNetIpMessage message)
        {
            KnxNetIpMessageReceived?.Invoke(this, message);
        }
    }
}