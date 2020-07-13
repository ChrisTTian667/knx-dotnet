using Knx.Common;

namespace Knx.DatapointTypes.Dpt1Bit
{
    [DatapointType(1, 2, Unit.TrueFalse, Usage.General)]
    public class DptBoolean : Dpt1Bit
    {
        public DptBoolean(byte[] payload)
            : base(payload)
        {
        }

        public DptBoolean(bool value)
            : base(value)
        {
        }

        [BooleanEncoding(UnitEncoding.False, UnitEncoding.True)]
        [DatapointProperty]
        public override bool Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}