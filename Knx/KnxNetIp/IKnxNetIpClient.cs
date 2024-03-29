using System;
using System.Threading;
using System.Threading.Tasks;

namespace Knx.KnxNetIp;

public interface IKnxNetIpClient :
    IAsyncDisposable,
    IObservable<KnxNetIpMessage>,
    IObservable<IKnxMessage>
{
    Task ConnectAsync();

    Task SendMessageAsync(IKnxMessage knxMessage, CancellationToken cancellationToken = default);

    event EventHandler<IKnxMessage> KnxMessageReceived;
}
