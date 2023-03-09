using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteUnsignedValue
{
    [DatapointType(7, 12, Unit.mA, Usage.FunctionBlock)]
    public class DptUEICurrentmA : Dpt2ByteUnsignedValue
    {
        public DptUEICurrentmA(byte[] payload)
            : base(payload)
        {
        }

        public DptUEICurrentmA(ushort value)
            : base(value)
        {
        }
    }
}