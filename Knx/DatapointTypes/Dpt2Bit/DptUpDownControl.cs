using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2Bit
{
    [DatapointType(2, 8, Unit.UpDown, Usage.FunctionBlock)]
    public class DptUpDownControl : Dpt2Bit
    {
        private DptUpDownControl()
        {
        }

        public DptUpDownControl(byte[] payload)
            : base(payload)
        {
        }

        public DptUpDownControl(bool value, bool control)
            : base(value, control)
        {
        }

        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.Down, UnitEncoding.Up)]
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