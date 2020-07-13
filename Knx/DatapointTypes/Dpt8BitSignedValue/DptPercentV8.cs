using Knx.Common;

namespace Knx.DatapointTypes.Dpt8BitSignedValue
{
    [DatapointType(6,1, Unit.Percentage, Usage.General)]
    public class DptPercentV8 : Dpt8BitSignedValue
    {
        public DptPercentV8(byte[] payload) : base(payload)
        {
        }

        public DptPercentV8(sbyte value) : base(value)
        {
        }
    }
}