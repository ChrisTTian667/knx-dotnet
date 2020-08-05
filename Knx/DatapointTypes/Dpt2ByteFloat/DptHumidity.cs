using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteFloat
{
    [DatapointType(9, 7, Unit.Percentage, Usage.General)]
    public class DptHumidity : Dpt2ByteFloat
    {
        private DptHumidity()
        {
        }

        public DptHumidity(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptHumidity(double value)
            : base(value)
        {
        }

        [DatapointProperty]
        [Range(0, +670760, ErrorMessage = "Humidity out of Range")]
        public override double Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}