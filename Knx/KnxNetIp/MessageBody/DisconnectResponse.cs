using Knx.Common;

namespace Knx.KnxNetIp.MessageBody;

public class DisconnectResponse : TunnelingMessageBody
{
    #region Properties

    /// <summary>
    ///     Gets or sets the state of the connection.
    /// </summary>
    /// <value>The state.</value>
    public ErrorCode State { get; private set; }

    public override KnxNetIpServiceType ServiceType => KnxNetIpServiceType.DisconnectResponse;

    #endregion

    #region Public Methods

    /// <summary>
    ///     Deserializes the specified bytes.
    /// </summary>
    /// <param name="bytes">The bytes.</param>
    public override void Deserialize(byte[] bytes)
    {
        CommunicationChannel = bytes[0];
        //this.State = (ErrorCode)Enum.Parse(typeof(ErrorCode), (((int)bytes[1]).ToString()));
        State = (ErrorCode)bytes[1];
    }

    /// <summary>
    ///     Serialize to ByteArray.
    /// </summary>
    /// <param name="byteArrayBuilder">The byte array builder.</param>
    public override void ToByteArray(ByteArrayBuilder byteArrayBuilder)
    {
        byteArrayBuilder.AddByte(CommunicationChannel).AddByte((byte)State);
    }

    #endregion
}