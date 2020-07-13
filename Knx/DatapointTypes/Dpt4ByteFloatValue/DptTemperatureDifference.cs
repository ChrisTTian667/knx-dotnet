using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 70, Unit.Kelvin, Usage.General, Description = "temperature difference")]
    public class DptTemperatureDifference : Dpt4ByteFloat
    {
        public DptTemperatureDifference(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptTemperatureDifference(float value)
            : base(value)
        {
        }
    }
}