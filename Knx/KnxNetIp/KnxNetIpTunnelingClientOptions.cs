using System;
using System.Net;

namespace Knx.KnxNetIp;

public class KnxNetIpTunnelingClientOptions
{
    /// <summary>
    ///     The IPAddress of the remote gateway
    /// </summary>
    public IPAddress RemoteAddress { get; set; } = IPAddress.Any;

    /// <summary>
    // The port of the remote gateway
    /// </summary>
    public int RemotePort { get; set; } = 3671;

    /// <summary>
    ///     The knx device address of the remote gateway
    /// </summary>
    public KnxDeviceAddress DeviceAddress { get; set; } = "0/0/0";

    /// <summary>
    ///     Timeout for the message acknowledge
    /// </summary>
    public TimeSpan AcknowledgeTimeout { get; set; } = TimeSpan.FromSeconds(3);
}