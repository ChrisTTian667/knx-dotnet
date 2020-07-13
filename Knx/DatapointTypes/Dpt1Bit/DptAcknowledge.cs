using Knx.Common;

namespace Knx.DatapointTypes.Dpt1Bit
{
    [DatapointType(1, 16, Unit.Acknowledge, Usage.General)]
    public class DptAcknowledge : Dpt1Bit
    {
        public DptAcknowledge(byte[] payload)
            : base(payload)
        {
        }

        public DptAcknowledge(bool value)
            : base(value)
        {
        }

        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.NoAction, UnitEncoding.Acknowledge)]
        public override bool Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}