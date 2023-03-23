using Knx.Common;
using Knx.Exceptions;
using Knx.ExtendedMessageInterface;

namespace Knx.KnxNetIp.MessageBody;

/// <summary>
///     Defines the message body of a TunnelingRequest message.
/// </summary>
[ResponseMessage(typeof(TunnelingAcknowledge))]
public class TunnelingRequest : TunnelingMessageBody
{
    public override KnxNetIpServiceType ServiceType => KnxNetIpServiceType.TunnelingRequest;

    /// <summary>
    ///     Gets or sets the cemi.
    /// </summary>
    /// <value>The cemi.</value>
    public IKnxMessage Cemi { get; set; }

    /// <summary>
    ///     Gets or sets the sequence counter.
    /// </summary>
    /// <value>The sequence counter.</value>
    [SequenceCounter(IncrementOnSendMessage = true)]
    public byte SequenceCounter { get; set; }

    /// <summary>
    ///     Gets the length of the communication header.
    ///     (SequenceCounter + CommunicationChannel)
    /// </summary>
    /// <value>The length.</value>
    private static byte Length => 4;

    public override string ToString()
    {
        return string.Format(
            "{0} Seq: {1,-3} Msg: {2}",
            base.ToString(),
            SequenceCounter,
            Cemi != null ? Cemi.ToString() : "empty");
    }

    /// <summary>
    ///     Deserializes the specified bytes.
    /// </summary>
    /// <param name="bytes">The bytes.</param>
    public override void Deserialize(byte[] bytes)
    {
        if (bytes[0] != 4) throw new KnxException("Could not parse ConnectionRequest: " + bytes);

        CommunicationChannel = bytes[1];
        SequenceCounter = bytes[2];
        // bytes[3] is reserved (should be always 0x00)

        // starting at byte index 4
        Cemi = KnxMessage.Deserialize(bytes.ExtractBytes(4));
    }

    /// <summary>
    ///     Serialize to ByteArray.
    /// </summary>
    /// <param name="byteArrayBuilder">The byte array builder.</param>
    internal override void ToByteArray(ByteArrayBuilder byteArrayBuilder)
    {
        byteArrayBuilder
            .AddByte(Length)
            .AddByte(CommunicationChannel)
            .AddByte(SequenceCounter)
            .AddByte(0) // reserved
            .Add(Cemi.ToByteArray());
    }
}