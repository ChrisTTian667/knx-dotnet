using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 46, Unit.MagneticFluxDensity, Usage.General, Description = "magnetic flux density")]
public class DptMagneticFluxDensity : Dpt4ByteFloat
{
    private DptMagneticFluxDensity()
    {
    }

    public DptMagneticFluxDensity(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptMagneticFluxDensity(float value)
        : base(value)
    {
    }
}