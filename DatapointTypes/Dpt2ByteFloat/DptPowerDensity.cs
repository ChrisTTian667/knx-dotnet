using Knx.Common;

namespace Knx.DatapointTypes.Dpt2ByteFloat
{
    [DatapointType(9, 22, Unit.PowerDensity, Usage.FunctionBlock)]
    public class DptPowerDensity : Dpt2ByteFloat
    {
        public DptPowerDensity(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptPowerDensity(double value)
            : base(value)
        {
        }

        [DatapointProperty]
        [Range(-670760, +670760, ErrorMessage = "Power density current out of Range")]
        public override double Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}