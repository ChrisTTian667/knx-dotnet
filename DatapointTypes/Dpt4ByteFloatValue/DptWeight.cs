using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 78, Unit.Weight, Usage.General, Description = "weight")]
    public class DptWeight : Dpt4ByteFloat
    {
        public DptWeight(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptWeight(float value)
            : base(value)
        {
        }
    }
}