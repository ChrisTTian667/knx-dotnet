using Knx.Common;
using Knx.ExtendedMessageInterface;

namespace Knx.KnxNetIp.MessageBody;

/// <summary>
///     Represents a routing indication for routing services.
/// </summary>
/// <remarks>
///     The data format contained in routing messages is cEMI. It is used to send a cEMI
///     message over IP networks. The routing indication service is unconfirmed.
/// </remarks>
public class RoutingIndication : MessageBodyBase
{
    public override KnxNetIpServiceType ServiceType => KnxNetIpServiceType.RoutingIndication;

    /// <summary>
    ///     Gets or sets the cemi.
    /// </summary>
    /// <value>The cemi.</value>
    public IKnxMessage Cemi { get; set; }

    public override void Deserialize(byte[] bytes)
    {
        Cemi = KnxMessage.Deserialize(bytes.ExtractBytes(0));
    }

    public override void ToByteArray(ByteArrayBuilder byteArrayBuilder)
    {
        byteArrayBuilder.Add(Cemi.ToByteArray());
    }

    public override string ToString()
    {
        return string.Format("{0} Msg: {1}", base.ToString(), Cemi != null ? Cemi.ToString() : "empty");
    }
}