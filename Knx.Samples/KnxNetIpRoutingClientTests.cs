using System.Diagnostics;
using System.Net;
using Knx.DatapointTypes.Dpt1Bit;
using Knx.ExtendedMessageInterface;
using Knx.KnxNetIp;

namespace Knx.Samples;

public class KnxNetIpRoutingClientTests
{
    public async Task ConnectTest()
    {
        using var target = new KnxNetIpRoutingClient();
        await target.Connect();
    }

    public async Task SendKnxMessage()
    {
        var target = new KnxNetIpRoutingClient();

        try
        {
            await target.Connect();

            var message = new KnxMessage
            {
                MessageType = MessageType.Write,
                MessageCode = MessageCode.Request,
                Priority = MessagePriority.Auto,
                SourceAddress = new KnxDeviceAddress(1, 1, 2),
                DestinationAddress = new KnxLogicalAddress(1, 1, 28),
                TransportLayerControlInfo = TransportLayerControlInfo.UnnumberedDataPacket,
                DataPacketCount = 0,
                Payload = new DptBoolean(false).Payload
            };

            await target.SendMessageAsync(message);
        }
        finally
        {
            target.Dispose();
        }
    }

    public async Task DiscoveryTest()
    {
        var taskCompletionSource = new TaskCompletionSource<string>();

        var target = new KnxNetIpRoutingClient();
        var localEndpoint = (IPEndPoint)await target.Connect();

        var searchRequest = MessageFactory.GetSearchRequest(localEndpoint);

        target.KnxDeviceDiscovered += (sender, info) =>
        {
            Debug.WriteLine("Device discovered: " + info);
            taskCompletionSource.TrySetResult(info.ToString());
        };

        await target.SendMessageAsync(searchRequest);

        await taskCompletionSource.Task;
    }
}
