using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 60, Unit.Resistance, Usage.General, Description = "resistance")]
    public class DptResistance : Dpt4ByteFloat
    {
        public DptResistance(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptResistance(float value)
            : base(value)
        {
        }
    }
}