using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 48, Unit.MagneticPolarization, Usage.General, Description = "magnetic polarization")]
    public class DptMagneticPolarization : Dpt4ByteFloat
    {
        public DptMagneticPolarization(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptMagneticPolarization(float value)
            : base(value)
        {
        }
    }
}