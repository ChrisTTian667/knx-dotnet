using Knx.Common;

namespace Knx.DatapointTypes.Dpt1Bit
{
    [DatapointType(1, 18, Unit.Occupancy, Usage.General)]
    public class DptOccupancy : Dpt1Bit
    {
        public DptOccupancy(byte[] payload)
            : base(payload)
        {
        }

        public DptOccupancy(bool value)
            : base(value)
        {
        }

        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.NotOccupied, UnitEncoding.Occupied)]
        public override bool Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}