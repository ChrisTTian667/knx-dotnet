using System;
using System.Runtime.Serialization;
using Knx.Common;

namespace Knx.DatapointTypes.Dpt4Bit
{
    [DataContract]
    [DataLength(4)]
    public abstract class Dpt4Bit : DatapointType
    {
        protected Dpt4Bit()
        {
        }

        protected Dpt4Bit(Byte[] payload)
            : base(payload)
        {
        }
    }
}
