using System;

namespace Knx
{
    public interface IMulticastUdpClient : IUdpClient
    {
        event EventHandler<BytesReceivedEventArgs> Received;
    }
}