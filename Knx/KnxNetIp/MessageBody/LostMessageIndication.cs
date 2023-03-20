using Knx.Common;

namespace Knx.KnxNetIp.MessageBody;

/// <summary>
///     Represents an indication for lost routing messages for routing services.
/// </summary>
/// <remarks>
///     The routing lost message is used to inform about the fact, that routing messages were
///     lost due to an overflow in the LAN-to-KNX queue, i.e. a router receives more IP packets
///     than it is able to deliver to the KNX network.
///     Additionally, the router device state is supplied.
/// </remarks>
public class LostMessageIndication : MessageBodyBase
{
    /// <summary>
    ///     Gets the length of the communication header.
    ///     (SequenceCounter + CommunicationChannel)
    /// </summary>
    /// <value>The length.</value>
    private static byte Length => 4;

    /// <summary>
    ///     The count of lost messages
    /// </summary>
    public int Lost { get; set; }

    /// <summary>
    ///     Gets or Sets the router device state,
    ///     i.e. the property referred to by PID 69 of object type 11 in the KNX property table.
    /// </summary>
    public byte DeviceState { get; set; }

    public override KnxNetIpServiceType ServiceType => KnxNetIpServiceType.RoutingLostMessage;

    public override void Deserialize(byte[] bytes)
    {
        DeviceState = bytes[1];
        Lost = (bytes[2] << 8) + bytes[3];
    }

    internal override void ToByteArray(ByteArrayBuilder byteArrayBuilder)
    {
        byteArrayBuilder
            .AddByte(Length)
            .AddByte(DeviceState)
            .AddInt(Lost);
    }
}
