using Knx.Common;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointType(20, 4, Usage.General)]
    public class DptPriority : Dpt8BitEnum<Priority>
    {
        public DptPriority(byte[] payload)
            : base(payload)
        {
        }

        public DptPriority(Priority value)
            : base(value)
        {
        }
    }
}