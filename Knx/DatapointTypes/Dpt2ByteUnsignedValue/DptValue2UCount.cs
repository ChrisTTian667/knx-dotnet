using System;
using Knx.Common;

namespace Knx.DatapointTypes.Dpt2ByteUnsignedValue
{
    [DatapointType(7,1,Unit.Pulses, Usage.General)]
    public class DptValue2UCount : Dpt2ByteUnsignedValue
    {
        public DptValue2UCount(byte[] payload) : base(payload)
        {
        }

        public DptValue2UCount(UInt16 value) : base(value)
        {
        }
    }
}