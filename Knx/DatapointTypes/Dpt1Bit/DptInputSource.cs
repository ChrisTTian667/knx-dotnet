using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit
{
    [DatapointType(1, 14, Unit.CalculatedFixed, Usage.FunctionBlock)]
    public class DptInputSource : Dpt1Bit
    {
        public DptInputSource(byte[] payload)
            : base(payload)
        {
        }

        public DptInputSource(bool value)
            : base(value)
        {
        }

        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.Fixed, UnitEncoding.Calculated)]
        public override bool Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}