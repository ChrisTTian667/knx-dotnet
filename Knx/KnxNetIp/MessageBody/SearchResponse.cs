using System;
using Knx.Common;

namespace Knx.KnxNetIp.MessageBody;

public class SearchResponse : TunnelingMessageBody
{
    public override KnxNetIpServiceType ServiceType => KnxNetIpServiceType.SearchResponse;

    public KnxHpai Endpoint { get; set; }


    #region Public Methods

    public override void Deserialize(byte[] bytes)
    {
        Endpoint = KnxHpai.Parse(bytes);
    }

    public override void ToByteArray(ByteArrayBuilder byteArrayBuilder)
    {
        throw new NotImplementedException();
    }

    #endregion
}
