using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteFloat
{
    [DatapointType(9, 23, Unit.KelvinPerPercent, Usage.FunctionBlock)]
    public class DptKelvinPerPercent : Dpt2ByteFloat
    {
        public DptKelvinPerPercent(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptKelvinPerPercent(double value)
            : base(value)
        {
        }

        [DatapointProperty]
        [Range(-670760, +670760, ErrorMessage = "Kelvin / Percent out of Range")]
        public override double Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}