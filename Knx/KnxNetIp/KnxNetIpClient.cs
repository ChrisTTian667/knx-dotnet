using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Knx.KnxNetIp;

public abstract class KnxNetIpClient : IDisposable
{
    protected readonly UdpClient UdpClient;

    protected KnxNetIpClient(
        IPEndPoint remoteEndPoint,
        KnxDeviceAddress deviceAddress,
        KnxNetIpConfiguration configuration = null)
    {
        Configuration = configuration ?? new KnxNetIpConfiguration();
        RemoteEndPoint = remoteEndPoint ?? throw new ArgumentNullException(nameof(remoteEndPoint));
        DeviceAddress = deviceAddress;

        UdpClient = new UdpClient();
    }

    public IPEndPoint RemoteEndPoint { get; }
    public KnxNetIpConfiguration Configuration { get; }
    public KnxDeviceAddress DeviceAddress { get; }

    /// <summary>
    ///     Returns a bool, indicating if the client is currently connected.
    /// </summary>
    public abstract bool IsConnected { get; protected set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~KnxNetIpClient() =>
        Dispose(false);

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            UdpClient.Dispose();
    }

    /// <summary>
    ///     Occurs when [KNX message received].
    /// </summary>
    public event EventHandler<IKnxMessage> KnxMessageReceived;

    /// <summary>
    ///     Connects the client to the bus
    /// </summary>
    /// <returns></returns>
    public virtual Task<EndPoint> Connect()
    {
        UdpClient.Connect(RemoteEndPoint);

        var localEndPoint = UdpClient.Client.LocalEndPoint;

        ReceiveData(UdpClient);

        return Task.FromResult(localEndPoint);
    }

    /// <summary>
    ///     Disconnects from Knx bus
    /// </summary>
    /// <returns></returns>
    public abstract Task Disconnect();

    /// <summary>
    ///     Sends a KnxMessage
    /// </summary>
    /// <param name="knxMessage"></param>
    public abstract Task SendMessageAsync(IKnxMessage knxMessage);

    /// <summary>
    ///     Receives the data from the UDP client.
    /// </summary>
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
    ///     Invoked when a KnxMessage has been received
    /// </summary>
    /// <param name="knxMessage"></param>
    protected void InvokeKnxMessageReceived(IKnxMessage knxMessage) =>
        KnxMessageReceived?.Invoke(this, knxMessage);

    /// <summary>
    ///     Invoked when a new KnxNetIpMessage has been retrieved.
    /// </summary>
    /// <param name="message"></param>
    protected virtual void OnKnxNetIpMessageReceived(KnxNetIpMessage message)
    {
        if (message != null)
            Debug.WriteLine("{0} RECV <= {1} (HANDLED)", DateTime.Now.ToLongTimeString(), message);
    }
}
