using Knx.Common;

namespace Knx.DatapointTypes.Dpt2Bit
{
    [DatapointType(2, 1, Unit.OnOff, Usage.General)]
    public class DptSwitchControl : Dpt2Bit
    {
        public DptSwitchControl(byte[] payload) : base(payload)
        {
        }

        public DptSwitchControl(bool value, bool control) : base(value, control)
        {
        }

        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.Off, UnitEncoding.On)]
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