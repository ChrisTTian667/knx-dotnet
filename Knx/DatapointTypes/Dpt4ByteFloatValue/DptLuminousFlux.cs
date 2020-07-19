using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 42, Unit.LuminousFlux, Usage.General, Description = "luminous flux")]
    public class DptLuminousFlux : Dpt4ByteFloat
    {
        public DptLuminousFlux(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptLuminousFlux(float value)
            : base(value)
        {
        }
    }
}