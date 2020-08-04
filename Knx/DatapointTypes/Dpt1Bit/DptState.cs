using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit
{
    [DatapointType(1, 11, Unit.ActiveInactive, Usage.FunctionBlock)]
    public class DptState : Dpt1Bit
    {
        private DptState()
        {
        }
        
        public DptState(byte[] payload)
            : base(payload)
        {
        }

        public DptState(bool value)
            : base(value)
        {
        }

        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.Inactive, UnitEncoding.Active)]
        public override bool Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}