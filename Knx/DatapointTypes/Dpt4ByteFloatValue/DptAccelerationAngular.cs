using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 1, Unit.AccelarationAngular, Usage.General, Description = "acceleration, angular")]
    public class DptAccelerationAngular : Dpt4ByteFloat
    {
        public DptAccelerationAngular(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptAccelerationAngular(float value)
            : base(value)
        {
        }
    }
}