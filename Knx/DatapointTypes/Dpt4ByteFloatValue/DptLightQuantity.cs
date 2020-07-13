using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 40, Unit.LightQuantity, Usage.General, Description = "light, quantity of")]
    public class DptLightQuantity : Dpt4ByteFloat
    {
        public DptLightQuantity(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptLightQuantity(float value)
            : base(value)
        {
        }
    }
}