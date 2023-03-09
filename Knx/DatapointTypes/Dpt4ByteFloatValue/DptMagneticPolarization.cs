using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 48, Unit.MagneticPolarization, Usage.General, Description = "magnetic polarization")]
public class DptMagneticPolarization : Dpt4ByteFloat
{
    private DptMagneticPolarization()
    {
    }

    public DptMagneticPolarization(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptMagneticPolarization(float value)
        : base(value)
    {
    }
}