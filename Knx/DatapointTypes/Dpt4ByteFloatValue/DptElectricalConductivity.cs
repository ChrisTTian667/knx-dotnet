using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 16, Unit.ElectricalConductivity, Usage.General, Description = "conductivity, electrical")]
    public class DptElectricalConductivity : Dpt4ByteFloat
    {
        public DptElectricalConductivity(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptElectricalConductivity(float value)
            : base(value)
        {
        }
    }
}