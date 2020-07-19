using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteFloat
{
    [DatapointType(9, 21,Unit.mA, Usage.General)]
    public class DptElectricalCurrent : Dpt2ByteFloat
    {
        public DptElectricalCurrent(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptElectricalCurrent(double value)
            : base(value)
        {
        }

        [DatapointProperty]
        [Range(-670760, +670760, ErrorMessage = "Electrical current out of Range")]
        public override double Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}