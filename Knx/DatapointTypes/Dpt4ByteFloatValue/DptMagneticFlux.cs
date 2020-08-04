using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 45, Unit.MagneticFlux, Usage.General, Description = "magnetic flux")]
    public class DptMagneticFlux : Dpt4ByteFloat
    {
        private DptMagneticFlux()
        {
        }

        public DptMagneticFlux(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptMagneticFlux(float value)
            : base(value)
        {
        }
    }
}