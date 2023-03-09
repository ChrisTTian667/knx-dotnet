using Knx.Common;

namespace Knx.KnxNetIp.MessageBody;

public class ConnectionStateResponse : TunnelingMessageBody
{
    /// <summary>
    ///     Gets or sets the state.
    /// </summary>
    /// <value>The state.</value>
    public ErrorCode State { get; private set; }

    public override KnxNetIpServiceType ServiceType => KnxNetIpServiceType.ConnectionStateResponse;

    public override void Deserialize(byte[] bytes)
    {
        CommunicationChannel = bytes[0];
        //this.State = (ErrorCode)Enum.Parse(typeof(ErrorCode), (((int)bytes[1]).ToString()));
        State = (ErrorCode)bytes[1];
    }

    public override void ToByteArray(ByteArrayBuilder byteArrayBuilder)
    {
        byteArrayBuilder.AddByte(CommunicationChannel).AddByte((byte)State);
    }
}