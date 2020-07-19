using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 56, Unit.Watt, Usage.General, Description = "power")]
    public class DptPower : Dpt4ByteFloat
    {
        public DptPower(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptPower(float value)
            : base(value)
        {
        }
    }
}