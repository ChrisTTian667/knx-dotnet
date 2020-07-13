using Knx.Common;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointType(20, 7, Usage.FunctionBlock)]
    public class DptAlarmClassType : Dpt8BitEnum<AlarmClassType>
    {
        public DptAlarmClassType(byte[] payload)
            : base(payload)
        {
        }

        public DptAlarmClassType(AlarmClassType value)
            : base(value)
        {
        }
    }
}