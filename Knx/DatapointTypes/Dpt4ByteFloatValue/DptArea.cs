using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 10,Unit.SquareMeter, Usage.General, Description = "area")]
    public class DptArea : Dpt4ByteFloat
    {
        public DptArea(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptArea(float value)
            : base(value)
        {
        }
    }
}