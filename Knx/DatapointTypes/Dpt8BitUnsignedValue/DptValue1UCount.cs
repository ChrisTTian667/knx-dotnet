using Knx.Common;

namespace Knx.DatapointTypes.Dpt8BitUnsignedValue
{
    [DatapointType(5,10,Unit.CounterPulses, Usage.General)]
    public class DptValue1UCount : Dpt8BitUnsignedValue
    {
        public DptValue1UCount(byte[] payload) : base(payload)
        {
        }

        public DptValue1UCount(int value) : base(value)
        {
        }

        [DatapointProperty]
        [Range(0, 255, ErrorMessage = "Counter pulses must be within 0 and 255")]
        public override int Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}