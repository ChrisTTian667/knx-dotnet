using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteFloat
{
    [DatapointType(9, 4, Unit.Lux, Usage.General)]
    public class DptLightIntensity : Dpt2ByteFloat
    {
        private DptLightIntensity()
        {
        }

        public DptLightIntensity(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptLightIntensity(double value)
            : base(value)
        {
        }

        [DatapointProperty]
        [Range(0, +670760, ErrorMessage = "Light intensity out of Range")]
        public override double Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}