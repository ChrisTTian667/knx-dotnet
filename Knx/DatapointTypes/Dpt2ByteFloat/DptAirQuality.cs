using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteFloat
{
    [DatapointType(9, 8, Unit.Ppm, Usage.General)]
    public class DptAirQuality : Dpt2ByteFloat
    {
        private DptAirQuality()
        {
        }

        public DptAirQuality(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptAirQuality(double value)
            : base(value)
        {
        }

        [DatapointProperty]
        [Range(0, +670760, ErrorMessage = "Air quality out of Range")]
        public override double Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}