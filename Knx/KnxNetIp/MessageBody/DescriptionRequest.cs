using System;
using Knx.Common;

namespace Knx.KnxNetIp.MessageBody
{
    [ResponseMessage(typeof(DescriptionResponse))]
    public class DescriptionRequest : TunnelingMessageBody
    {
        public override KnxNetIpServiceType ServiceType
        {
            get { return KnxNetIpServiceType.DescriptionRequest; }
        }

        #region Public Methods

        public override void Deserialize(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public override void ToByteArray(ByteArrayBuilder byteArrayBuilder)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}