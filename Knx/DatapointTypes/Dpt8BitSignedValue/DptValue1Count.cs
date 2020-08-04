using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitSignedValue
{
    [DatapointType(6, 10, Unit.CounterPulses, Usage.General)]
    public class DptValue1Count : Dpt8BitSignedValue
    {
        private DptValue1Count()
        {
        }

        public DptValue1Count(byte[] payload)
            : base(payload)
        {
        }

        public DptValue1Count(sbyte value)
            : base(value)
        {
        }
    }
}