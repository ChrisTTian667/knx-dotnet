using System.Net;
using Knx.KnxNetIp;
using Knx.KnxNetIp.MessageBody;

namespace Knx.Tests
{
    internal static class MockObjects
    {
        public static KnxHpai DefaultHpai =>
            new KnxHpai
            {
                HostProtocolCode = HostProtocolCode.IPV4_UDP,
                IpAddress = IPAddress.Parse("192.168.2.1"),
                Port = 3060
            };

        public static ConnectionRequest ConnectionRequestMock =>
            new ConnectionRequest
            {
                CommunicationChannel = 1,
                ControlEndpoint = DefaultHpai,
                Data = new ConnectRequestData
                {
                    ConnectionType = ConnectionType.TunnelingConnection,
                    NetIpLayer = KnxNetIpLayer.BusMonitor
                },
                DataEndpoint = DefaultHpai
            };
    }
}