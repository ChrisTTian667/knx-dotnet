using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitUnsignedValue
{
    [DatapointType(5, 4, Unit.Percentage, Usage.FunctionBlock)]
    public class DptPercentU8 : Dpt8BitUnsignedValue
    {
        private DptPercentU8()
        {
        }

        public DptPercentU8(byte[] payload)
            : base(payload)
        {
        }

        public DptPercentU8(int value)
            : base(value)
        {
        }

        [DatapointProperty]
        [Range(0, 255, ErrorMessage = "Percentage value must be within 0 and 255")]
        public override int Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}