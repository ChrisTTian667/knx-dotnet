using Knx.Common;

namespace Knx.KnxNetIp.MessageBody;

public abstract class MessageBodyBase
{
    public abstract KnxNetIpServiceType ServiceType { get; }

    /// <summary>
    ///     Deserializes the specified bytes.
    /// </summary>
    /// <param name="bytes">The bytes.</param>
    public abstract void Deserialize(byte[] bytes);

    /// <summary>
    ///     Serialize to ByteArray.
    /// </summary>
    /// <param name="byteArrayBuilder">The byte array builder.</param>
    public abstract void ToByteArray(ByteArrayBuilder byteArrayBuilder);
}