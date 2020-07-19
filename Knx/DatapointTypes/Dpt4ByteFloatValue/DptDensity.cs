using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 17, Unit.Density, Usage.General, Description = "density")]
    public class DptDensity : Dpt4ByteFloat
    {
        public DptDensity(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptDensity(float value)
            : base(value)
        {
        }
    }
}