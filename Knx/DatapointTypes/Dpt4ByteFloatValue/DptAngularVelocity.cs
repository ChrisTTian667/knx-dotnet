using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 9, Unit.AngularVelocity, Usage.General, Description = "angular velocity")]
    public class DptAngularVelocity : Dpt4ByteFloat
    {
        private DptAngularVelocity()
        {
        }

        public DptAngularVelocity(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptAngularVelocity(float value)
            : base(value)
        {
        }
    }
}