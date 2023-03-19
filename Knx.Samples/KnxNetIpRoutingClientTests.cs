using System.Diagnostics;
using Knx.DatapointTypes.Dpt1Bit;
using Knx.ExtendedMessageInterface;
using Knx.KnxNetIp;

namespace Knx.Samples;

public class KnxNetIpRoutingClientTests
{
    public async Task ConnectTest()
    {
        await using var target = new KnxNetIpRoutingClient();
        await target.ConnectAsync();
    }

    public async Task SendKnxMessage()
    {
        await using var routingClient = new KnxNetIpRoutingClient();

        await routingClient.ConnectAsync();

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

        await routingClient.SendMessageAsync(message);
    }

    public async Task DiscoveryTest()
    {
        var taskCompletionSource = new TaskCompletionSource<string>();

        var target = new KnxNetIpRoutingClient(
            options =>
            {
                options.DeviceAddress = new KnxDeviceAddress(1, 1, 2);
            });

        target.KnxDeviceDiscovered += (_, info) =>
        {
            Debug.WriteLine("Device discovered: " + info);
            taskCompletionSource.TrySetResult(info.ToString());
        };

        await target.DiscoverAsync();

        await taskCompletionSource.Task;
    }
}
