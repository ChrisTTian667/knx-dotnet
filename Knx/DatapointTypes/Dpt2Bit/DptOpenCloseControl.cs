using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2Bit
{
    [DatapointType(2, 9, Unit.OpenClose, Usage.FunctionBlock)]
    public class DptOpenCloseControl : Dpt2Bit
    {
        public DptOpenCloseControl(byte[] payload)
            : base(payload)
        {
        }

        public DptOpenCloseControl(bool value, bool control)
            : base(value, control)
        {
        }

        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.Close, UnitEncoding.Open)]
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