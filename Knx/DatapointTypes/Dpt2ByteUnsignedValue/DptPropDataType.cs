using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteUnsignedValue
{
    [DatapointType(7, 10, Usage.General)]
    public class DptPropDataType : Dpt2ByteUnsignedValue
    {
        public DptPropDataType(byte[] payload)
            : base(payload)
        {
        }

        public DptPropDataType(UInt16 value)
            : base(value)
        {
        }
    }
}