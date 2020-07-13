using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 7, Unit.Degrees, Usage.General, Description = "angle, degree")]
    public class DptAngleDegree : Dpt4ByteFloat
    {
        public DptAngleDegree(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptAngleDegree(float value)
            : base(value)
        {
        }
    }
}