using System;
using Knx.Common;

namespace Knx.KnxNetIp.MessageBody;

[ResponseMessage(typeof(ConnectionStateResponse))]
public class ConnectionStateRequest : TunnelingMessageBody
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ConnectionStateRequest" /> class.
    /// </summary>
    public ConnectionStateRequest()
    {
        HostProtocolAddressInfo = new KnxHpai();
    }

    /// <summary>
    ///     Gets or sets the host protocol address info.
    /// </summary>
    /// <value>The host protocol address info.</value>
    public KnxHpai HostProtocolAddressInfo { get; private set; }

    public override KnxNetIpServiceType ServiceType => KnxNetIpServiceType.ConnectionStateRequest;

    /// <summary>
    ///     Deserializes the specified bytes.
    /// </summary>
    /// <param name="bytes">The bytes.</param>
    public override void Deserialize(byte[] bytes)
    {
        var hpaiLength = (int)bytes[2]; // get the length for the "HostProtocolAddressInformation //
        var hpaiBytes = new byte[hpaiLength]; // extract the hpai bytes
        Array.Copy(bytes, 2, hpaiBytes, 0, hpaiLength); // parse the host protocol address information

        CommunicationChannel = bytes[0];
        HostProtocolAddressInfo = KnxHpai.Parse(hpaiBytes);
    }

    /// <summary>
    ///     Serialize to ByteArray.
    /// </summary>
    /// <param name="byteArrayBuilder">The byte array builder.</param>
    internal override void ToByteArray(ByteArrayBuilder byteArrayBuilder)
    {
        byteArrayBuilder.AddByte(CommunicationChannel)
            .AddByte((byte)ErrorCode.NoError)
            .Add(
                HostProtocolAddressInfo.ToByteArray());
    }
}
