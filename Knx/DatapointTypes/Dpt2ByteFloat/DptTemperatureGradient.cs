using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteFloat
{
    [DatapointType(9, 3, Unit.TemperatureGradient, Usage.General)]
    public class DptTemperatureGradient : Dpt2ByteFloat
    {
        public DptTemperatureGradient(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptTemperatureGradient(double value)
            : base(value)
        {
        }

        [DatapointProperty]
        [Range(-670760, +670760, ErrorMessage = "Temperature gradient out of Range")]
        public override double Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}