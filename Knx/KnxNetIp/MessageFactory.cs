using System;
using System.Net;
using Knx.KnxNetIp.MessageBody;

namespace Knx.KnxNetIp
{
    public static class MessageFactory
    {
        #region Public Methods

        public static KnxNetIpMessage CreateTunnelingRequestMessage()
        {
            var message = KnxNetIpMessage.Create(KnxNetIpServiceType.TunnelingRequest);
            return null;
        }

        public static KnxNetIpMessage GetTunnelingRequest()
        {
            return KnxNetIpMessage.Create(KnxNetIpServiceType.TunnelingRequest);
        }

        #endregion

        /*
        public KnxNetIpMessage CreateTestMessage(bool lightsOn)
        {
            var tunnelingRequest = KnxNetIpMessage.Create(KnxServiceType.TunnelingRequest);
            var tunnelBody = (tunnelingRequest.Body as KnxMessageBodyTunnelingRequest);

            if (tunnelBody != null)
            {
                tunnelBody.SequenceCounter = _sequenceCounter;
                tunnelBody.Cemi = new KnxMessage
                {
                    MessageCode = MessageCode.Request,
                    IsRepetition = false,
                    Priority = MessagePriority.System,
                    SourceAddress = new KnxDeviceAddress(0, 0, 0),
                    DataLength = 1,
                    DestinationAddress = new KnxLogicalAddress(3, 3, 0),
                    TransportLayerControlInfo = TransportLayerControlInfo.UnnumberedDataPacket,
                    DataPacketCount = 0,
                    LightsOn = lightsOn
                };
            }
            _sequenceCounter++;
            return tunnelingRequest;
        }
        */

        #region Methods

        internal static KnxNetIpMessage GetConnectRequest(IPEndPoint localEndPoint)
        {
            if (localEndPoint == null)
                throw new ArgumentNullException(nameof(localEndPoint));

            var msg = KnxNetIpMessage.Create(KnxNetIpServiceType.ConnectionRequest);

            if (msg.Body is ConnectionRequest body)
            {
                InitializeHostProtocolAddressInformation(body.ControlEndpoint, localEndPoint);
                InitializeHostProtocolAddressInformation(body.DataEndpoint, localEndPoint);
                body.Data.ConnectionType = ConnectionType.TunnelingConnection;
            }

            return msg;
        }

        public static KnxNetIpMessage GetSearchRequest(IPEndPoint localEndPoint)
        {
            if (localEndPoint == null)
                throw new ArgumentNullException("localEndPoint", "LocalEndpoint cannot be null");

            var msg = KnxNetIpMessage.Create(KnxNetIpServiceType.SearchRequest);
            var body = msg.Body as SearchRequest;

            if (body != null)
            {
                InitializeHostProtocolAddressInformation(body.Endpoint, localEndPoint);
            }

            return msg;
        }

        internal static KnxNetIpMessage GetConnectionStateRequest(IPEndPoint localEndPoint)
        {
            if (localEndPoint == null)
                throw new ArgumentNullException("localEndPoint", "LocalEndpoint cannot be null");

            var msg = KnxNetIpMessage.Create(KnxNetIpServiceType.ConnectionStateRequest);
            var body = msg.Body as ConnectionStateRequest;

            if (body != null)
            {
                InitializeHostProtocolAddressInformation(body.HostProtocolAddressInfo, localEndPoint);
            }

            return msg;
        }

        internal static KnxNetIpMessage GetDisconnectRequest(IPEndPoint localEndPoint, byte communicationChannel)
        {
            if (localEndPoint == null)
                throw new ArgumentNullException("localEndPoint", "LocalEndpoint cannot be null");

            var msg = KnxNetIpMessage.Create(KnxNetIpServiceType.DisconnectRequest);
            var body = msg.Body as DisconnectRequest;

            if (body != null)
            {
                InitializeHostProtocolAddressInformation(body.HostProtocolAddressInfo, localEndPoint);
            }

            return msg;
        }

        private static void InitializeHostProtocolAddressInformation(KnxHpai hostProtocolAddressInfo, IPEndPoint localEndPoint)
        {
            if(hostProtocolAddressInfo == null)
                throw new ArgumentNullException("hostProtocolAddressInfo", "KnxHpai cannot be null");
            if (localEndPoint == null)
                throw new ArgumentNullException("localEndPoint", "LocalEndpoint cannot be null");

            hostProtocolAddressInfo.HostProtocolCode = HostProtocolCode.IPV4_UDP;
            hostProtocolAddressInfo.IpAddress = localEndPoint.Address;
            hostProtocolAddressInfo.Port = localEndPoint.Port;
        }

        #endregion
    }


}