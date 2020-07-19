using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 67, Unit.SurfaceTension, Usage.General, Description = "surface tension")]
    public class DptSurfaceTension : Dpt4ByteFloat
    {
        public DptSurfaceTension(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptSurfaceTension(float value)
            : base(value)
        {
        }
    }
}