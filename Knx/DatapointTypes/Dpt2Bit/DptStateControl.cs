using Knx.Common;

namespace Knx.DatapointTypes.Dpt2Bit
{
    [DatapointType(2, 11, Unit.ActiveInactive, Usage.FunctionBlock)]
    public class DptStateControl : Dpt2Bit
    {
        public DptStateControl(byte[] payload)
            : base(payload)
        {
        }

        public DptStateControl(bool value, bool control)
            : base(value, control)
        {
        }

        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.Inactive, UnitEncoding.Active)]
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