using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitUnsignedValue
{
    [DatapointType(5, 2, Unit.Angle, Usage.General)]
    public class DptAngle : Dpt8BitUnsignedValue
    {
        public DptAngle(byte[] payload)
            : base(payload)
        {
        }

        public DptAngle(int value)
            : base(value)
        {
        }

        [DatapointProperty]
        [Range(0, 360, ErrorMessage = "Angle must be within 0 and 360")]
        public override int Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}