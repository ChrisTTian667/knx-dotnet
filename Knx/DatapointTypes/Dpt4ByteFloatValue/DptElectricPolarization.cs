using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 26, Unit.ElectricPolarization, Usage.General, Description = "electric polarization")]
    public class DptElectricPolarization : Dpt4ByteFloat
    {
        public DptElectricPolarization(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptElectricPolarization(float value)
            : base(value)
        {
        }
    }
}