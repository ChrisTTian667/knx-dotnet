using System;

namespace Knx.KnxNetIp;

public class KnxNetIpConfiguration
{
    public TimeSpan ReadTimeout { get; set; } = TimeSpan.FromSeconds(10);
    public TimeSpan SendMessageTimeout { get; set; } = TimeSpan.FromSeconds(3);
    public int MaxKeepAliveRetries { get; set; } = 3;
}