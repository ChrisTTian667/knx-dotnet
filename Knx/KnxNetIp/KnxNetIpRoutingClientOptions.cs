using System;
using System.Net;

namespace Knx.KnxNetIp;

public class KnxNetIpRoutingClientOptions
{
    private static readonly IPAddress DefaultMulticastAddress = IPAddress.Parse("224.0.23.12");

    /// <summary>
    /// The IPAddress of the remote gateway
    /// </summary>
    public IPAddress RemoteAddress { get; set; } = DefaultMulticastAddress;

    /// <summary>
    /// The port of the remote gateway
    /// </summary>
    public int RemotePort { get; set; } = 3671;

    /// <summary>
    /// The knx device address of the remote gateway
    /// </summary>
    public KnxDeviceAddress DeviceAddress { get; set; } = "0/0/0";

    /// <summary>
    /// Default read message timeout
    /// </summary>
    [Obsolete("This has nothing to do with the client itself")]
    public TimeSpan ReadMessageTimeout { get; set; } = TimeSpan.FromSeconds(10);

    /// <summary>
    /// Default send message timeout
    /// </summary>
    [Obsolete("This has nothing to do with the client itself")]
    public TimeSpan SendMessageTimeout { get; set; } = TimeSpan.FromSeconds(3);

    /// <summary>
    /// Max retries for keep alive messages
    /// </summary>
    [Obsolete("This has nothing to do with the client itself")]
    public int MaxKeepAliveRetries { get; set; } = 3;
}
