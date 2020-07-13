using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 39, Unit.Meter, Usage.General, Description = "length")]
    public class DptLength : Dpt4ByteFloat
    {
        public DptLength(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptLength(float value)
            : base(value)
        {
        }
    }
}