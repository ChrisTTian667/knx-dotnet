using System;
using Knx.Common;

namespace Knx.KnxNetIp.MessageBody;

[ResponseMessage(typeof(DescriptionResponse))]
public class DescriptionRequest : TunnelingMessageBody
{
    public override KnxNetIpServiceType ServiceType => KnxNetIpServiceType.DescriptionRequest;

    public override void Deserialize(byte[] bytes)
    {
        throw new NotImplementedException();
    }

    internal override void ToByteArray(ByteArrayBuilder byteArrayBuilder)
    {
        throw new NotImplementedException();
    }
}