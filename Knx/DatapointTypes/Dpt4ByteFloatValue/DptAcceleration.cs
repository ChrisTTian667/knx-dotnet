using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14,0, Unit.Accelaration, Usage.General, Description = "acceleration")]
    public class DptAcceleration : Dpt4ByteFloat
    {
        public DptAcceleration(byte[] twoBytes) : base(twoBytes)
        {
        }

        public DptAcceleration(float value) : base(value)
        {
        }
    }
}