using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteSignedValue
{
    [DatapointType(8,1, Unit.Pulses, Usage.General)]
    public class DptValue2Count : Dpt2ByteSignedValue
    {
        private DptValue2Count()
        {
        }

        public DptValue2Count(byte[] payload) : base(payload)
        {
        }

        public DptValue2Count(short value) : base(value)
        {
        }
    }
}