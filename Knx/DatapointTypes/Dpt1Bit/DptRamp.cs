using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit
{
    [DatapointType(1, 4, Unit.RampNoRamp, Usage.FunctionBlock)]
    public class DptRamp : Dpt1Bit
    {
        private DptRamp()
        {
        }

        public DptRamp(byte[] payload)
            : base(payload)
        {
        }

        public DptRamp(bool value)
            : base(value)
        {
        }

        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.NoRamp, UnitEncoding.Ramp)]
        public override bool Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}