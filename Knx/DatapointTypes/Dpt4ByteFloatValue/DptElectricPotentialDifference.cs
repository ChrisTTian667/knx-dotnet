using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 28, Unit.ElectricPotentialDifference, Usage.General, Description = "electric potential")]
    public class DptElectricPotentialDifference : Dpt4ByteFloat
    {
        public DptElectricPotentialDifference(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptElectricPotentialDifference(float value)
            : base(value)
        {
        }
    }
}