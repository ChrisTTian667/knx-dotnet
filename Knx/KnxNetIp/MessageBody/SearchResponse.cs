using System;
using Knx.Common;

namespace Knx.KnxNetIp.MessageBody;

public class SearchResponse : TunnelingMessageBody
{
    public override KnxNetIpServiceType ServiceType => KnxNetIpServiceType.SearchResponse;

    public KnxHpai Endpoint { get; set; }


    public override void Deserialize(byte[] bytes)
    {
        Endpoint = KnxHpai.Parse(bytes);
    }

    internal override void ToByteArray(ByteArrayBuilder byteArrayBuilder)
    {
        throw new NotImplementedException();
    }
}
