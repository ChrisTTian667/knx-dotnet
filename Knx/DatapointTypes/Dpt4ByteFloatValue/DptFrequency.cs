using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 33, Unit.Frequency, Usage.General, Description = "frequency")]
    public class DptFrequency : Dpt4ByteFloat
    {
        public DptFrequency(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptFrequency(float value)
            : base(value)
        {
        }
    }
}