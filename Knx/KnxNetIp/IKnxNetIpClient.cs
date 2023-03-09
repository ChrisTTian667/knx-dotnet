using System;
using System.Threading.Tasks;

namespace Knx.KnxNetIp;

public interface IKnxNetIpClient
{
    KnxNetIpConfiguration Configuration { get; }
    KnxDeviceAddress DeviceAddress { get; }
    Task SendMessageAsync(IKnxMessage knxMessage);

    event EventHandler<IKnxMessage> KnxMessageReceived;
}
