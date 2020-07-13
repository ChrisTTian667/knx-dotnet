using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 22, Unit.ElectricalDisplacement, Usage.General, Description = "electric displacement")]
    public class DptElectricDisplacement : Dpt4ByteFloat
    {
        public DptElectricDisplacement(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptElectricDisplacement(float value)
            : base(value)
        {
        }
    }
}