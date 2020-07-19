using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 23, Unit.ElectricFieldStrength, Usage.General, Description = "electric field strength")]
    public class DptElectricFieldStrength : Dpt4ByteFloat
    {
        public DptElectricFieldStrength(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptElectricFieldStrength(float value)
            : base(value)
        {
        }
    }
}