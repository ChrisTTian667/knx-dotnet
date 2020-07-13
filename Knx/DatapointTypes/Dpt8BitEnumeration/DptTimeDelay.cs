using Knx.Common;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointType(20, 13, Usage.FunctionBlock)]
    public class DptTimeDelay : Dpt8BitEnum<TimeDelay>
    {
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