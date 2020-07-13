using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 50, Unit.MagnetomotiveForce, Usage.General, Description = "magneto motive force")]
    public class DptMagnetomotiveForce : Dpt4ByteFloat
    {
        public DptMagnetomotiveForce(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptMagnetomotiveForce(float value)
            : base(value)
        {
        }
    }
}