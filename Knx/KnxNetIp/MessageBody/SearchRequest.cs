using Knx.Common;

namespace Knx.KnxNetIp.MessageBody;

[ResponseMessage(typeof(SearchResponse))]
public class SearchRequest : TunnelingMessageBody
{
    public SearchRequest() =>
        Endpoint = new KnxHpai();

    public override KnxNetIpServiceType ServiceType =>
        KnxNetIpServiceType.SearchRequest;

    public KnxHpai Endpoint { get; }

    public override void Deserialize(byte[] bytes)
    {
        //throw new NotImplementedException();
    }

    public override void ToByteArray(ByteArrayBuilder byteArrayBuilder)
    {
        byteArrayBuilder.Add(Endpoint.ToByteArray());
    }
}
