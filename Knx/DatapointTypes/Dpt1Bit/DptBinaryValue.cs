using Knx.Common;

namespace Knx.DatapointTypes.Dpt1Bit
{
    [DatapointType(1, 6, Unit.HighLow, Usage.FunctionBlock)]
    public class DptBinaryValue : Dpt1Bit
    {
        public DptBinaryValue(byte[] payload)
            : base(payload)
        {
        }

        public DptBinaryValue(bool value)
            : base(value)
        {
        }

        [BooleanEncoding(UnitEncoding.Low, UnitEncoding.High)]
        [DatapointProperty]
        public override bool Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}