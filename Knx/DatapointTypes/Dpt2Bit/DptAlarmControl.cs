using System.Runtime.Serialization;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2Bit
{
    [DataContract]
    [DatapointType(2, 5, Unit.AlarmNoAlarm, Usage.FunctionBlock)]
    public class DptAlarmControl : Dpt2Bit
    {
        public DptAlarmControl(byte[] payload)
            : base(payload)
        {
        }

        public DptAlarmControl(bool value, bool control)
            : base(value, control)
        {
        }

        [DataMember]
        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.NoAlarm, UnitEncoding.Alarm)]
        public override bool Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                base.Value = value;
            }
        }
    }
}