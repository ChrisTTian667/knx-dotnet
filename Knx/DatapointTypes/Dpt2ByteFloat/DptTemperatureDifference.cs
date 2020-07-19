using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteFloat
{
    [DatapointType(9, 2, Unit.TemperatureDifference, Usage.General)]
    public class DptTemperatureDifference : Dpt2ByteFloat
    {
        public DptTemperatureDifference(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptTemperatureDifference(double value)
            : base(value)
        {
        }

        [DatapointProperty]
        [Range(-670760, +670760, ErrorMessage = "Temperature differnce out of Range")]
        public override double Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}