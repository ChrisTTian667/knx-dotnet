using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Knx.KnxNetIp.MessageBody;

namespace Knx.KnxNetIp;

/// <summary>
///     Used to connect to the Knx Bus via KnxNetIpRouting protocol.
/// </summary>
public class KnxNetIpRoutingClient : IKnxNetIpClient, IDisposable
{
    private readonly UdpClient _udpClient;

    public KnxNetIpRoutingClient(KnxNetIpConfiguration configuration = null) : this(
        new IPEndPoint(IPAddress.Parse("224.0.23.12"), 3671),
        new KnxDeviceAddress(0, 0, 0),
        configuration)
    {
    }

    public KnxNetIpRoutingClient(
        IPEndPoint remoteEndPoint,
        KnxDeviceAddress deviceAddress,
        KnxNetIpConfiguration configuration = null)
    {
        Configuration = configuration ?? new KnxNetIpConfiguration();
        RemoteEndPoint = remoteEndPoint ?? throw new ArgumentNullException(nameof(remoteEndPoint));
        DeviceAddress = deviceAddress;

        _udpClient = new UdpClient();
    }

    public IPEndPoint RemoteEndPoint { get; }

    /// <summary>
    ///     Gets a value indicating whether the client is connected.
    /// </summary>
    /// <value>
    ///     <c>true</c> if client is connected; otherwise, <c>false</c>.
    /// </value>
    public bool IsConnected { get; protected set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public KnxNetIpConfiguration Configuration { get; }
    public KnxDeviceAddress DeviceAddress { get; }


    /// <summary>
    ///     Occurs when [KNX message received].
    /// </summary>
    public event EventHandler<IKnxMessage> KnxMessageReceived;

    /// <summary>
    ///     Sends a KnxMessage.
    /// </summary>
    /// <param name="knxMessage">The KNX message.</param>
    public async Task SendMessageAsync(IKnxMessage knxMessage)
    {
        var knxNetIpMessage = KnxNetIpMessage.Create(KnxNetIpServiceType.RoutingIndication);
        ((RoutingIndication)knxNetIpMessage.Body).Cemi = knxMessage;

        await SendMessageAsync(knxNetIpMessage);
    }

    ~KnxNetIpRoutingClient()
    {
        Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
//        if (disposing)
//            UdpClient.Dispose();
    }

    /// <summary>
    ///     Called when knx device has been discovered.
    /// </summary>
    public event EventHandler<DeviceInfo> KnxDeviceDiscovered;

    public Task<EndPoint> BaseConnect()
    {
        _udpClient.Connect(RemoteEndPoint);

        var localEndPoint = _udpClient.Client.LocalEndPoint;

        ReceiveData(_udpClient);

        return Task.FromResult(localEndPoint);
    }

    private async void ReceiveData(UdpClient client)
    {
        KnxNetIpMessage lastMessage = null;

        var receivedBuffer = new List<byte>();
        while (true)
        {
            var receivedResult = await client.ReceiveAsync();
            var receivedData = receivedResult.Buffer.ToArray();
            receivedBuffer.AddRange(receivedData);

            if (!receivedBuffer.Any())
                continue;

            var msg = KnxNetIpMessage.Parse(receivedBuffer.ToArray());

            if (msg == null)
                continue;

            receivedBuffer.Clear();

            // verify that the message differ from last one.
            if (lastMessage != null && lastMessage.ServiceType == msg.ServiceType)
            {
                if (lastMessage.ToByteArray().SequenceEqual(msg.ToByteArray()))
                    continue;
            }

            OnKnxNetIpMessageReceived(msg);
            lastMessage = msg;
        }
    }

    /// <summary>
    ///     Connects this instance.
    /// </summary>
    public async Task<EndPoint> Connect()
    {
        try
        {
            _udpClient.MulticastLoopback = false;
            var localEndpoint = (IPEndPoint)await BaseConnect();
            _udpClient.JoinMulticastGroup(RemoteEndPoint.Address, localEndpoint.Address);

            var multiClient = new UdpClient(123, AddressFamily.InterNetwork);

            multiClient.JoinMulticastGroup(
                RemoteEndPoint.Address,
                localEndpoint.Address);

            var something = await multiClient.ReceiveAsync();


            return localEndpoint;
        }
        finally
        {
            IsConnected = true;
        }
    }

    protected void OnKnxNetIpMessageReceived(KnxNetIpMessage message)
    {
        Debug.WriteLine("{0} RECV <= {1} (HANDLED)", DateTime.Now.ToLongTimeString(), message);

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

    /// <summary>
    ///     Invoked when a KnxMessage has been received
    /// </summary>
    /// <param name="knxMessage"></param>
    protected void InvokeKnxMessageReceived(IKnxMessage knxMessage)
    {
        KnxMessageReceived?.Invoke(this, knxMessage);
    }


    private void InvokeKnxDeviceDiscovered(DeviceInfo knxDeviceInfo)
    {
        KnxDeviceDiscovered?.Invoke(this, knxDeviceInfo);
    }

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

    public Task Disconnect()
    {
        IsConnected = false;

        return Task.FromResult(true);
    }

    /// <summary>
    ///     Sends a KnxNetIpMessage
    /// </summary>
    /// <param name="message"></param>
    public async Task SendMessageAsync(KnxNetIpMessage message)
    {
        Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} SEND => {message}");

        var bytes = message.ToByteArray();
        await _udpClient.SendAsync(bytes, bytes.Length);
    }
}
