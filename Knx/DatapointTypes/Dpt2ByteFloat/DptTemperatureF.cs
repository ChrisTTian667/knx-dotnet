using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteFloat
{
    [DatapointType(9, 27, Unit.TemperatureF, Usage.General)]
    public class DptTemperatureF : Dpt2ByteFloat
    {
        public DptTemperatureF(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptTemperatureF(double value)
            : base(value)
        {
        }

        [DatapointProperty]
        [Range(-459.6, 670760.96, ErrorMessage = "Temperature Value out of Range")]
        public override double Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}