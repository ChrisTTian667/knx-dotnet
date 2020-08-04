using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 20, Unit.ElectricalCurrentDensity, Usage.General, Description = "electric current density")]
    public class DptElectricCurrentDensity : Dpt4ByteFloat
    {
        private DptElectricCurrentDensity()
        {
        }

        public DptElectricCurrentDensity(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptElectricCurrentDensity(float value)
            : base(value)
        {
        }
    }
}