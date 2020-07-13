using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 61, Unit.Resistivity, Usage.General, Description = "resistivity")]
    public class DptResistivity : Dpt4ByteFloat
    {
        public DptResistivity(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptResistivity(float value)
            : base(value)
        {
        }
    }
}