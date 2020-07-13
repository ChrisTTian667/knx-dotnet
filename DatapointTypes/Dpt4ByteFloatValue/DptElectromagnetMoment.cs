using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 29, Unit.ElectromagnetMoment, Usage.General, Description = "electromagnetic moment")]
    public class DptElectromagnetMoment : Dpt4ByteFloat
    {
        public DptElectromagnetMoment(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptElectromagnetMoment(float value)
            : base(value)
        {
        }
    }
}