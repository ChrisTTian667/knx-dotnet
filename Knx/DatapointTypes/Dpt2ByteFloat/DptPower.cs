using Knx.Common;

namespace Knx.DatapointTypes.Dpt2ByteFloat
{
    [DatapointType(9, 24, Unit.Power, Usage.FunctionBlock)]
    public class DptPower : Dpt2ByteFloat
    {
        public DptPower(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptPower(double value)
            : base(value)
        {
        }

        [DatapointProperty]
        [Range(-670760, +670760, ErrorMessage = "Power out of Range")]
        public override double Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}