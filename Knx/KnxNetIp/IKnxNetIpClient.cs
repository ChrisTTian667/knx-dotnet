using System;
using System.Threading;
using System.Threading.Tasks;

namespace Knx.KnxNetIp;

public interface IKnxNetIpClient
{
    Task SendMessageAsync(IKnxMessage knxMessage, CancellationToken cancellationToken = default);

    event EventHandler<IKnxMessage> KnxMessageReceived;
}
