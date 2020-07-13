using Knx.Common;

namespace Knx.DatapointTypes.Dpt2BitEnumeration
{
    [DatapointType(23, 2, Usage.FunctionBlock)]
    public class DptAlarmReaction : Dpt2BitEnum<AlarmReaction>
    {
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
