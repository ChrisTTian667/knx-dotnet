using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2Bit
{
    [DatapointType(2, 7, Unit.IncreaseDecrease, Usage.FunctionBlock)]
    public class DptStepControl : Dpt2Bit
    {
        private DptStepControl()
        {
        }

        public DptStepControl(byte[] payload)
            : base(payload)
        {
        }

        public DptStepControl(bool value, bool control)
            : base(value, control)
        {
        }

        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.Decrease, UnitEncoding.Increase)]
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