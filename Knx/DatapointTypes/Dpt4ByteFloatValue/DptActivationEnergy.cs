using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 2, Unit.ActivationEnergy, Usage.General, Description = "activation energy")]
    public class DptActivationEnergy : Dpt4ByteFloat
    {
        private DptActivationEnergy()
        {
        }

        public DptActivationEnergy(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptActivationEnergy(float value)
            : base(value)
        {
        }
    }
}