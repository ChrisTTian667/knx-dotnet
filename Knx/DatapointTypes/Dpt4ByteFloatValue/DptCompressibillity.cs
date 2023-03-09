using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 14, Unit.Compressibillity, Usage.General, Description = "compressibillity")]
    public class DptCompressibillity : Dpt4ByteFloat
    {
        public DptCompressibillity(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptCompressibillity(float value)
            : base(value)
        {
        }
    }
}