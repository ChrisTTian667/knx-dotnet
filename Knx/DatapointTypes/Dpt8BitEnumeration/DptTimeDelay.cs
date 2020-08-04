using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointType(20, 13, Usage.FunctionBlock)]
    public class DptTimeDelay : Dpt8BitEnum<TimeDelay>
    {
        private DptTimeDelay()
        {
        }

        public DptTimeDelay(byte[] payload)
            : base(payload)
        {
        }

        public DptTimeDelay(TimeDelay value)
            : base(value)
        {
        }
    }
}