using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 20, Unit.ElectricalCurrentDensity, Usage.General, Description = "electric current density")]
    public class DptElectricCurrentDensity : Dpt4ByteFloat
    {
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