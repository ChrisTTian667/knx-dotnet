using System;
using System.Net;
using System.Threading.Tasks;
using Knx.DatapointTypes;
using Knx.DatapointTypes.Dpt1Bit;
using Knx.DatapointTypes.Dpt2ByteFloat;
using Knx.ExtendedMessageInterface;
using Knx.KnxNetIp;
using NUnit.Framework;

namespace Knx.Tests;

public class KnxNetIpTunnelingClientTests
{
    public KnxMessage SwitchOfficeLightsOn(bool on)
    {
        return new KnxMessage
        {
            MessageType = MessageType.Write,
            MessageCode = MessageCode.Request,
            Priority = MessagePriority.Auto,
            SourceAddress = new KnxDeviceAddress(1, 1, 2),
            DestinationAddress = new KnxLogicalAddress(0, 1, 0),
            TransportLayerControlInfo = TransportLayerControlInfo.UnnumberedDataPacket,
            DataPacketCount = 0,
            Payload = new[] { on ? (byte)1 : (byte)0 }
        };
    }

    public KnxMessage ReadOfficeSetpointTemperature()
    {
        return new KnxMessage
        {
            MessageType = MessageType.Read,
            MessageCode = MessageCode.Request,
            Priority = MessagePriority.Auto,
            SourceAddress = new KnxDeviceAddress(1, 1, 2),
            DestinationAddress = new KnxLogicalAddress(5, 1, 34),
            TransportLayerControlInfo = TransportLayerControlInfo.UnnumberedDataPacket,
            DataPacketCount = 0,
            Payload = new byte[] { }
        };
    }

    public KnxMessage WriteOfficeSetpointTemperature()
    {
        return new KnxMessage
        {
            MessageType = MessageType.Write,
            MessageCode = MessageCode.Request,
            Priority = MessagePriority.Auto,
            SourceAddress = new KnxDeviceAddress(1, 1, 2),
            DestinationAddress = new KnxLogicalAddress(5, 1, 34),
            TransportLayerControlInfo = TransportLayerControlInfo.UnnumberedDataPacket,
            DataPacketCount = 0,
            Payload = new DptTemperature(22.3).Payload
        };
    }

    public KnxMessage WriteOfficeCurrentTime()
    {
        return new KnxMessage
        {
            MessageType = MessageType.Write,
            MessageCode = MessageCode.Request,
            Priority = MessagePriority.Auto,
            SourceAddress = new KnxDeviceAddress(1, 1, 2),
            DestinationAddress = new KnxLogicalAddress(9, 3, 0),
            TransportLayerControlInfo = TransportLayerControlInfo.UnnumberedDataPacket,
            DataPacketCount = 0,
            Payload = new DptTime(new TimeSpan(13, 37, 00), DayOfWeek.Monday).Payload
        };
    }

    public KnxMessage WriteOfficeCurrentDate()
    {
        return new KnxMessage
        {
            MessageType = MessageType.Write,
            MessageCode = MessageCode.Request,
            Priority = MessagePriority.Auto,
            SourceAddress = new KnxDeviceAddress(1, 1, 2),
            DestinationAddress = new KnxLogicalAddress(9, 3, 1),
            TransportLayerControlInfo = TransportLayerControlInfo.UnnumberedDataPacket,
            DataPacketCount = 0,
            Payload = new DptDate(new DateTime(2011, 09, 11)).Payload
        };
    }

    [Test]
    public async Task ConnectTest()
    {
        using var target = new KnxNetIpTunnelingClient(
            new IPEndPoint(IPAddress.Parse("10.0.2.5"), 3671),
            KnxAddress.Device(1, 1, 2));

        await target.Connect();
    }

    [Test]
    public async Task SendKnxMessage()
    {
        var target = new KnxNetIpTunnelingClient(
            new IPEndPoint(IPAddress.Parse("10.10.10.11"), 3671),
            KnxAddress.Device(1, 1, 2));

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

            // test for simpler SendMessage calls
            //target.Write(KnxAddress.Logical(9, 3, 0), (new DptTime(new TimeSpan(13, 36, 00), DayOfWeek.Monday)));
        }
        finally
        {
            target.Dispose();
        }
    }
}
