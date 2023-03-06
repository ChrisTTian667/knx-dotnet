using Knx.Common;

namespace Knx.KnxNetIp.MessageBody;

public class TunnelingAcknowledge : TunnelingMessageBody
{
    public override string ToString()
    {
        return string.Format("{0} Seq: {1,-3} Err: {2}", base.ToString(), SequenceCounter, State);
    }

    /// <summary>
    ///     Gets or sets the length.
    /// </summary>
    /// <value>The length.</value>
    public byte Length { get; set; }

    /// <summary>
    ///     Gets or sets the sequence counter.
    /// </summary>
    /// <value>The sequence counter.</value>
    [SequenceCounter(IncrementOnSendMessage = false)]
    public byte SequenceCounter { get; set; }

    /// <summary>
    ///     Gets or sets the state.
    /// </summary>
    /// <value>The state.</value>
    public ErrorCode State { get; set; }

    public override KnxNetIpServiceType ServiceType => KnxNetIpServiceType.TunnelingAcknowledge;

    /// <summary>
    ///     Deserializes the specified bytes.
    /// </summary>
    /// <param name="bytes">The bytes.</param>
    public override void Deserialize(byte[] bytes)
    {
        Length = bytes[0];
        CommunicationChannel = bytes[1];
        SequenceCounter = bytes[2];
        //this.State = (ErrorCode)Enum.Parse(typeof(ErrorCode), bytes[3].ToString());
        State = (ErrorCode)bytes[3];
    }

    /// <summary>
    ///     Serialize to ByteArray.
    /// </summary>
    /// <param name="byteArrayBuilder">The byte array builder.</param>
    public override void ToByteArray(ByteArrayBuilder byteArrayBuilder)
    {
        byteArrayBuilder.AddByte(Length).AddByte(CommunicationChannel).AddByte(SequenceCounter).AddByte((byte)State);
    }
}