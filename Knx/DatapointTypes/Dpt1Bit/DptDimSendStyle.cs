using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit
{
    [DatapointType(1, 13, Unit.CyclicallyOrStartStop, Usage.FunctionBlock)]
    public class DptDimSendStyle : Dpt1Bit
    {
        public DptDimSendStyle(byte[] payload)
            : base(payload)
        {
        }

        public DptDimSendStyle(bool value)
            : base(value)
        {
        }

        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.StartStop, UnitEncoding.Cyclically)]
        public override bool Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}