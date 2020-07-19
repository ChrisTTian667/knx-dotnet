using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 8, Unit.AngularMomentum, Usage.General, Description = "angular momentum")]
    public class DptAngularMomentum : Dpt4ByteFloat
    {
        public DptAngularMomentum(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptAngularMomentum(float value)
            : base(value)
        {
        }
    }
}