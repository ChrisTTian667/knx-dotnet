using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 27, Unit.ElectricPotential, Usage.General, Description = "electric potential")]
    public class DptElectricPotential : Dpt4ByteFloat
    {
        private DptElectricPotential()
        {
        }

        public DptElectricPotential(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptElectricPotential(float value)
            : base(value)
        {
        }
    }
}