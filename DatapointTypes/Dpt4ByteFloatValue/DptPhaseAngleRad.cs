using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 54, Unit.AngleRad, Usage.General, Description = "phase angle, radiant")]
    public class DptPhaseAngleRad : Dpt4ByteFloat
    {
        public DptPhaseAngleRad(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptPhaseAngleRad(float value)
            : base(value)
        {
        }
    }
}