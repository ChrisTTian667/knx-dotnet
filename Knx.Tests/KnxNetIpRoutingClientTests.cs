using System.Threading.Tasks;
using Knx.DatapointTypes.Dpt1Bit;
using Knx.ExtendedMessageInterface;
using Knx.KnxNetIp;
using NUnit.Framework;

namespace Knx.Tests
{
    public class KnxNetIpRoutingClientTests
    {
        [Test]
        public async Task ConnectTest()
        {
            using (var target = new KnxNetIpRoutingClient())
            {
                await target.Connect();
            }
        }

        [Test]
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

                await target.SendMessage(message);
            }
            finally
            {
                target.Dispose();
            }
        }
    }
}