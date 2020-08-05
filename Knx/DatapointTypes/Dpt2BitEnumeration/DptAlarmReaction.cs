using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2BitEnumeration
{
    [DatapointType(23, 2, Usage.FunctionBlock)]
    public class DptAlarmReaction : Dpt2BitEnum<AlarmReaction>
    {
        private DptAlarmReaction()
        {
        }
       
        public DptAlarmReaction(byte[] payload)
            : base(payload)
        {
        }

        public DptAlarmReaction(AlarmReaction value)
            : base(value)
        {
        }
    }
}
