using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 6, Unit.AngleRad, Usage.General, Description = "angle, radiant")]
    public class DptAngleRad : Dpt4ByteFloat
    {
        public DptAngleRad(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptAngleRad(float value)
            : base(value)
        {
        }
    }
}