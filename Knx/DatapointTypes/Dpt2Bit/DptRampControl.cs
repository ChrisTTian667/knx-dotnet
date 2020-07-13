using Knx.Common;

namespace Knx.DatapointTypes.Dpt2Bit
{
    [DatapointType(2, 4, Unit.RampNoRamp, Usage.FunctionBlock)]
    public class DptRampControl : Dpt2Bit
    {
        public DptRampControl(byte[] payload)
            : base(payload)
        {
        }

        public DptRampControl(bool value, bool control)
            : base(value, control)
        {
        }

        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.NoRamp, UnitEncoding.Ramp)]
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