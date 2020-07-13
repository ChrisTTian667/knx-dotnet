using Knx.Common;

namespace Knx.DatapointTypes.Dpt2Bit
{
    [DatapointType(2, 10, Unit.StartStop, Usage.FunctionBlock)]
    public class DptStartStopControl : Dpt2Bit
    {
        public DptStartStopControl(byte[] payload)
            : base(payload)
        {
        }

        public DptStartStopControl(bool value, bool control)
            : base(value, control)
        {
        }

        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.Stop, UnitEncoding.Start)]
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