using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteUnsignedValue
{
    [DatapointType(7,11, Unit.Millimeter, Usage.FunctionBlock)]
    public class DptLengthInMillimeter : Dpt2ByteUnsignedValue
    {
        private DptLengthInMillimeter()
        {
        }

        public DptLengthInMillimeter(byte[] payload) : base(payload)
        {
        }

        public DptLengthInMillimeter(ushort value) : base(value)
        {
        }
    }
}