using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 74, Unit.Seconds, Usage.General, Description = "time")]
    public class DptTime : Dpt4ByteFloat
    {
        public DptTime(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptTime(float value)
            : base(value)
        {
        }
    }
}