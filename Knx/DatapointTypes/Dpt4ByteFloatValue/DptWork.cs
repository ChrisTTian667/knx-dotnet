using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 79, Unit.Work, Usage.General, Description = "work")]
    public class DptWork : Dpt4ByteFloat
    {
        private DptWork()
        {
        }

        public DptWork(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptWork(float value)
            : base(value)
        {
        }
    }
}