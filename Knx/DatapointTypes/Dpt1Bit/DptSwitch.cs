using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit
{
    [DatapointType(1, 1, Unit.OnOff, Usage.General)]
    public class DptSwitch : Dpt1Bit
    {
        private DptSwitch()
        {
        }
        
        public DptSwitch(byte[] payload) : base(payload)
        {
        }

        public DptSwitch(bool value) : base(value)
        {
        }

        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.Off, UnitEncoding.On)]
        public override bool Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}