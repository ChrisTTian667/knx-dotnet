using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2Bit
{
    [DatapointType(2, 12, Unit.InvertedNotInverted, Usage.FunctionBlock)]
    public class DptInvertedControl : Dpt2Bit
    {
        private DptInvertedControl()
        {
        }

        public DptInvertedControl(byte[] payload)
            : base(payload)
        {
        }

        public DptInvertedControl(bool value, bool control)
            : base(value, control)
        {
        }

        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.NotInverted, UnitEncoding.Inverted)]
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