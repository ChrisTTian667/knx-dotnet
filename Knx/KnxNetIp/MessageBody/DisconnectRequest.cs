using System;
using Knx.Common;

namespace Knx.KnxNetIp.MessageBody;

/// <summary>
///     Disconnect Request MessageBody
/// </summary>
[ResponseMessage(typeof(DisconnectResponse))]
public class DisconnectRequest : TunnelingMessageBody
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DisconnectRequest" /> class.
    /// </summary>
    public DisconnectRequest()
    {
        HostProtocolAddressInfo = new KnxHpai();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DisconnectRequest" /> class.
    /// </summary>
    /// <param name="communicationChannel">The communication channel.</param>
    /// <param name="hostProtocolAddressInfo">The host protocol address info.</param>
    public DisconnectRequest(byte communicationChannel, KnxHpai hostProtocolAddressInfo)
    {
        CommunicationChannel = communicationChannel;
        HostProtocolAddressInfo = hostProtocolAddressInfo;
    }

    /// <summary>
    ///     Gets or sets the host protocol address info.
    /// </summary>
    /// <value>The host protocol address info.</value>
    public KnxHpai HostProtocolAddressInfo { get; set; }

    public override KnxNetIpServiceType ServiceType => KnxNetIpServiceType.DisconnectRequest;

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
    public override void ToByteArray(ByteArrayBuilder byteArrayBuilder)
    {
        byteArrayBuilder.AddByte(CommunicationChannel)
            .AddByte((byte)ErrorCode.NoError)
            .Add(
                HostProtocolAddressInfo.ToByteArray());
    }
}