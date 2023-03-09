using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 44, Unit.MagneticFieldStrength, Usage.General, Description = "magnetic field strength")]
public class DptMagneticFieldStrength : Dpt4ByteFloat
{
    private DptMagneticFieldStrength()
    {
    }

    public DptMagneticFieldStrength(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptMagneticFieldStrength(float value)
        : base(value)
    {
    }
}