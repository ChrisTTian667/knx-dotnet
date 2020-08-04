using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteUnsignedValue
{
    [DatapointType(12, 1, Unit.Pulses, Usage.General)]
    public class DptValue4UCount : Dpt4ByteUnsignedValue
    {
        private DptValue4UCount()
        {
        }

        public DptValue4UCount(byte[] payload) : base(payload)
        {
        }

        public DptValue4UCount(uint value) : base(value)
        {
        }
    }
}