using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 49, Unit.Magnetization, Usage.General, Description = "magnetization")]
    public class DptMagnetization : Dpt4ByteFloat
    {
        public DptMagnetization(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptMagnetization(float value)
            : base(value)
        {
        }
    }
}