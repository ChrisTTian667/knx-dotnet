using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Knx.KnxNetIp.MessageBody;

namespace Knx.KnxNetIp;

/// <summary>
///     Used to connect to the Knx Bus via KnxNetIpRouting protocol.
/// </summary>
public class KnxNetIpRoutingClient : KnxNetIpClient
{
    public KnxNetIpRoutingClient(KnxNetIpConfiguration configuration = null) : this(
        new IPEndPoint(IPAddress.Parse("224.0.23.12"), 3671),
        new KnxDeviceAddress(0, 0, 0),
        configuration)
    {
    }

    public KnxNetIpRoutingClient(
        IPEndPoint remoteEndPoint,
        KnxDeviceAddress deviceAddress,
        KnxNetIpConfiguration configuration = null) : base(remoteEndPoint, deviceAddress, configuration)
    {
    }

    /// <summary>
    ///     Gets a value indicating whether the client is connected.
    /// </summary>
    /// <value>
    ///     <c>true</c> if client is connected; otherwise, <c>false</c>.
    /// </value>
    public override bool IsConnected { get; protected set; }

    /// <summary>
    ///     Called when knx device has been discovered.
    /// </summary>
    public event EventHandler<DeviceInfo> KnxDeviceDiscovered;

    /// <summary>
    ///     Connects this instance.
    /// </summary>
    public override async Task<EndPoint> Connect()
    {
        try
        {
            UdpClient.MulticastLoopback = false;
            var localEndpoint = (IPEndPoint)await base.Connect();
            UdpClient.JoinMulticastGroup(RemoteEndPoint.Address, localEndpoint.Address);

            var multiClient = new UdpClient(123, AddressFamily.InterNetwork);
            multiClient.JoinMulticastGroup(RemoteEndPoint.Address, localEndpoint.Address);
            var something = await multiClient.ReceiveAsync();


            return localEndpoint;
        }
        finally
        {
            IsConnected = true;
        }
    }

    protected override void OnKnxNetIpMessageReceived(KnxNetIpMessage message)
    {
        base.OnKnxNetIpMessageReceived(message);

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
    }

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

    public override Task Disconnect()
    {
        IsConnected = false;

        return Task.FromResult(true);
    }

    /// <summary>
    ///     Sends a KnxMessage.
    /// </summary>
    /// <param name="knxMessage">The KNX message.</param>
    public override async Task SendMessageAsync(IKnxMessage knxMessage)
    {
        var knxNetIpMessage = KnxNetIpMessage.Create(KnxNetIpServiceType.RoutingIndication);
        ((RoutingIndication)knxNetIpMessage.Body).Cemi = knxMessage;

        await SendMessageAsync(knxNetIpMessage);
    }

    /// <summary>
    ///     Sends a KnxNetIpMessage
    /// </summary>
    /// <param name="message"></param>
    public async Task SendMessageAsync(KnxNetIpMessage message)
    {
        Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} SEND => {message}");

        var bytes = message.ToByteArray();
        await UdpClient.SendAsync(bytes, bytes.Length);
    }
}
