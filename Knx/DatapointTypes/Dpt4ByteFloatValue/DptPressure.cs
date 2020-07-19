using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 58, Unit.Pa, Usage.General, Description = "pressure")]
    public class DptPressure : Dpt4ByteFloat
    {
        public DptPressure(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptPressure(float value)
            : base(value)
        {
        }
    }
}