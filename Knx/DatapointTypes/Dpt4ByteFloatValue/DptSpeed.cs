using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 65, Unit.Speed, Usage.General, Description = "speed")]
    public class DptSpeed : Dpt4ByteFloat
    {
        public DptSpeed(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptSpeed(float value)
            : base(value)
        {
        }
    }
}