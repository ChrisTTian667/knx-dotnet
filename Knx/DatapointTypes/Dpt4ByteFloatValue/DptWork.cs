using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 79, Unit.Work, Usage.General, Description = "work")]
    public class DptWork : Dpt4ByteFloat
    {
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