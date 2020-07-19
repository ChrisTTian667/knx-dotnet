using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2Bit
{
    [DatapointType(2, 3, Unit.EnableDisable, Usage.FunctionBlock)]
    public class DptEnableControl : Dpt2Bit
    {
        public DptEnableControl(byte[] payload)
            : base(payload)
        {
        }

        public DptEnableControl(bool value, bool control)
            : base(value, control)
        {
        }

        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.Disable, UnitEncoding.Enable)]
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