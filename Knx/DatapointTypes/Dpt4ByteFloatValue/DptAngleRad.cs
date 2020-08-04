using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 6, Unit.AngleRad, Usage.General, Description = "angle, radiant")]
    public class DptAngleRad : Dpt4ByteFloat
    {
        private DptAngleRad()
        {
        }

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