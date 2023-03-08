using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Knx.KnxNetIp;

public interface IKnxNetIpClient
{
    Task SendMessageAsync(IKnxMessage knxMessage);

    KnxNetIpConfiguration Configuration { get; }
    KnxDeviceAddress DeviceAddress { get; }

    event EventHandler<IKnxMessage> KnxMessageReceived;
}
