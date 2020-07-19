using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 68, Unit.Temperature, Usage.General, Description = "common temperature")]
    public class DptCommonTemperature : Dpt4ByteFloat
    {
        public DptCommonTemperature(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptCommonTemperature(float value)
            : base(value)
        {
        }
    }
}