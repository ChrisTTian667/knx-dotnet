using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteFloat
{
    [DatapointType(9, 5, Unit.WindSpeed, Usage.General)]
    public class DptWindSpeed : Dpt2ByteFloat
    {
        private DptWindSpeed()
        {
        }

        public DptWindSpeed(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptWindSpeed(double value)
            : base(value)
        {
        }

        [DatapointProperty]
        [Range(0, +670760, ErrorMessage = "Windspeed out of Range")]
        public override double Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}