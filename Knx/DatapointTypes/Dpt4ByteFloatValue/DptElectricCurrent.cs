using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 19, Unit.ElectricalCurrent, Usage.General, Description = "electric current")]
    public class DptElectricCurrent : Dpt4ByteFloat
    {
        private DptElectricCurrent()
        {
        }

        public DptElectricCurrent(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptElectricCurrent(float value)
            : base(value)
        {
        }
    }
}