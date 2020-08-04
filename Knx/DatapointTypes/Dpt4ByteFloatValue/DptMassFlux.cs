using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 52, Unit.MassFlux, Usage.General, Description = "mass flux")]
    public class DptMassFlux : Dpt4ByteFloat
    {
        private DptMassFlux()
        {
        }

        public DptMassFlux(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptMassFlux(float value)
            : base(value)
        {
        }
    }
}