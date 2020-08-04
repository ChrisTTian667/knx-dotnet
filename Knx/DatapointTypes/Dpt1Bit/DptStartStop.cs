using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit
{
    [DatapointType(1, 10, Unit.StartStop, Usage.General)]
    public class DptStartStop : Dpt1Bit
    {
        private DptStartStop()
        {
        } 
        
        public DptStartStop(byte[] payload)
            : base(payload)
        {
        }

        public DptStartStop(bool value)
            : base(value)
        {
        }

        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.Stop, UnitEncoding.Start)]
        public override bool Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}