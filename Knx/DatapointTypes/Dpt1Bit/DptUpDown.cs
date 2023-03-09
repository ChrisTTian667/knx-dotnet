using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit
{
    [DatapointType(1, 8, Unit.UpDown, Usage.General)]
    public class DptUpDown : Dpt1Bit
    {
        public DptUpDown(byte[] payload)
            : base(payload)
        {
        }

        public DptUpDown(bool value)
            : base(value)
        {
        }

        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.Down, UnitEncoding.Up)]
        public override bool Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}