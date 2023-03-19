using Knx.Common;

namespace Knx.KnxNetIp.MessageBody;

public class TunnelingAcknowledge : TunnelingMessageBody
{
    public override string ToString() =>
        $"{base.ToString()} Seq: {SequenceCounter,-3} Err: {State}";

    /// <summary>
    ///     Gets or sets the length.
    /// </summary>
    public byte Length { get; set; }

    /// <summary>
    ///     Gets or sets the sequence counter.
    /// </summary>
    [SequenceCounter(IncrementOnSendMessage = false)]
    public byte SequenceCounter { get; set; }

    /// <summary>
    ///     Gets or sets the state.
    /// </summary>
    public ErrorCode State { get; set; }

    public override KnxNetIpServiceType ServiceType => KnxNetIpServiceType.TunnelingAcknowledge;

    /// <summary>
    ///     Deserializes the specified bytes.
    /// </summary>
    public override void Deserialize(byte[] bytes)
    {
        Length = bytes[0];
        CommunicationChannel = bytes[1];
        SequenceCounter = bytes[2];
        //State = (ErrorCode)Enum.Parse(typeof(ErrorCode), bytes[3].ToString());
        State = (ErrorCode)bytes[3];
    }

    /// <summary>
    ///     Serialize to ByteArray.
    /// </summary>
    public override void ToByteArray(ByteArrayBuilder byteArrayBuilder) =>
        byteArrayBuilder
            .AddByte(Length)
            .AddByte(CommunicationChannel)
            .AddByte(SequenceCounter)
            .AddByte((byte)State);
}
