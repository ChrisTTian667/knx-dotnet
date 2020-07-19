using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 18, Unit.ElectricalCharge, Usage.General, Description = "electrical charge")]
    public class DptElectricCharge : Dpt4ByteFloat
    {
        public DptElectricCharge(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptElectricCharge(float value)
            : base(value)
        {
        }
    }
}