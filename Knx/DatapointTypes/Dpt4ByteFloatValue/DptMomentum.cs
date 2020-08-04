using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 53, Unit.Momentum, Usage.General, Description = "momentum")]
    public class DptMomentum : Dpt4ByteFloat
    {
        private DptMomentum()
        {
        }

        public DptMomentum(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptMomentum(float value)
            : base(value)
        {
        }
    }
}