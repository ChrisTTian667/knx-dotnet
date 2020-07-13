using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 2, Unit.ActivationEnergy, Usage.General, Description = "activation energy")]
    public class DptActivationEnergy : Dpt4ByteFloat
    {
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