using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 55, Unit.Angle, Usage.General, Description = "phase angle, degrees")]
    public class DptPhaseAngleDeg : Dpt4ByteFloat
    {
        public DptPhaseAngleDeg(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptPhaseAngleDeg(float value)
            : base(value)
        {
        }
    }
}