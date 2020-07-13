using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 57, Unit.PowerFactor, Usage.General, Description = "power factor")]
    public class DptPowerFactor : Dpt4ByteFloat
    {
        public DptPowerFactor(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptPowerFactor(float value)
            : base(value)
        {
        }
    }
}