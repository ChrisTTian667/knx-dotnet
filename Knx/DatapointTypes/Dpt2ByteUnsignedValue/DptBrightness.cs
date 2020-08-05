using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteUnsignedValue
{
    [DatapointType(7, 13, Unit.Lux, Usage.FunctionBlock)]
    public class DptBrightness : Dpt2ByteUnsignedValue
    {
        private DptBrightness()
        {
        }
        
        public DptBrightness(byte[] payload)
            : base(payload)
        {
        }

        public DptBrightness(ushort value)
            : base(value)
        {
        }
    }
}