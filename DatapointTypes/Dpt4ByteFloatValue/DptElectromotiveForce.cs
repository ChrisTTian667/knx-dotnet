using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 30, Unit.ElectromotiveForce, Usage.General, Description = "electromotive force")]
    public class DptElectromotiveForce : Dpt4ByteFloat
    {
        public DptElectromotiveForce(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptElectromotiveForce(float value)
            : base(value)
        {
        }
    }
}