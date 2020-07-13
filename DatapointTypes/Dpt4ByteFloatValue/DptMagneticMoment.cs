using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 47, Unit.MagneticMoment, Usage.General, Description = "magnetic moment")]
    public class DptMagneticMoment : Dpt4ByteFloat
    {
        public DptMagneticMoment(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptMagneticMoment(float value)
            : base(value)
        {
        }
    }
}