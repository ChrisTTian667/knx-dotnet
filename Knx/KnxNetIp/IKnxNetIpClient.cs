using System;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace Knx.KnxNetIp;

public interface IKnxNetIpClient :
    IAsyncDisposable,
    ISubject<KnxNetIpMessage>,
    ISubject<IKnxMessage>
{
    Task ConnectAsync();

    Task SendMessageAsync(IKnxMessage knxMessage, CancellationToken cancellationToken = default);

    event EventHandler<IKnxMessage> KnxMessageReceived;
}
