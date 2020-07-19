using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit
{
    [DatapointType(1, 5, Unit.AlarmNoAlarm, Usage.FunctionBlock)]
    public class DptAlarm : Dpt1Bit
    {
        public DptAlarm(byte[] payload)
            : base(payload)
        {
        }

        public DptAlarm(bool value)
            : base(value)
        {
        }

        [BooleanEncoding(UnitEncoding.NoAlarm, UnitEncoding.Alarm)]
        [DatapointProperty]
        public override bool Value
        {
            get => base.Value;
            set => base.Value = value;
        }
    }
}