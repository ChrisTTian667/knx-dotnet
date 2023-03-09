﻿using System;
using Knx.Common;

namespace Knx.KnxNetIp.MessageBody
{
    public class DescriptionResponse : TunnelingMessageBody
    {
        public override KnxNetIpServiceType ServiceType
        {
            get { return KnxNetIpServiceType.DescriptionResponse; }
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