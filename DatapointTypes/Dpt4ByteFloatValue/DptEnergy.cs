using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 31, Unit.Energy, Usage.General, Description = "energy")]
    public class DptEnergy : Dpt4ByteFloat
    {
        public DptEnergy(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptEnergy(float value)
            : base(value)
        {
        }
    }
}