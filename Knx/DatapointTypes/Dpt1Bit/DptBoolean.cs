using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit
{
    [DatapointType(1, 2, Unit.TrueFalse, Usage.General)]
    public class DptBoolean : Dpt1Bit
    {
        private DptBoolean()
        {
        }
        
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
            get => base.Value;
            set => base.Value = value;
        }
    }
}