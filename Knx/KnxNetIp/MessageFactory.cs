using System;
using System.Net;
using Knx.KnxNetIp.MessageBody;

namespace Knx.KnxNetIp;

public static class MessageFactory
{
    public static KnxNetIpMessage CreateTunnelingRequestMessage()
    {
        return KnxNetIpMessage.Create(KnxNetIpServiceType.TunnelingRequest);
    }

    public static KnxNetIpMessage GetTunnelingRequest()
    {
        return KnxNetIpMessage.Create(KnxNetIpServiceType.TunnelingRequest);
    }

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
        var msg = KnxNetIpMessage.Create(KnxNetIpServiceType.SearchRequest);
        if (msg.Body is SearchRequest body)
            InitializeHostProtocolAddressInformation(body.Endpoint, localEndPoint);

        return msg;
    }

    internal static KnxNetIpMessage GetConnectionStateRequest(IPEndPoint localEndPoint)
    {
        var msg = KnxNetIpMessage.Create(KnxNetIpServiceType.ConnectionStateRequest);
        if (msg.Body is ConnectionStateRequest body)
            InitializeHostProtocolAddressInformation(body.HostProtocolAddressInfo, localEndPoint);

        return msg;
    }

    internal static KnxNetIpMessage GetDisconnectRequest(IPEndPoint localEndPoint, byte communicationChannel)
    {
        var msg = KnxNetIpMessage.Create(KnxNetIpServiceType.DisconnectRequest);
        if (msg.Body is DisconnectRequest body)
            InitializeHostProtocolAddressInformation(body.HostProtocolAddressInfo, localEndPoint);

        return msg;
    }

    private static void InitializeHostProtocolAddressInformation(
        KnxHpai hostProtocolAddressInfo,
        IPEndPoint localEndPoint)
    {
        hostProtocolAddressInfo.HostProtocolCode = HostProtocolCode.IPV4_UDP;
        hostProtocolAddressInfo.IpAddress = localEndPoint.Address;
        hostProtocolAddressInfo.Port = localEndPoint.Port;
    }
}
