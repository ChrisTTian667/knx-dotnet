using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 51, Unit.Kilogram, Usage.General, Description = "mass")]
    public class DptMass : Dpt4ByteFloat
    {
        public DptMass(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptMass(float value)
            : base(value)
        {
        }
    }
}