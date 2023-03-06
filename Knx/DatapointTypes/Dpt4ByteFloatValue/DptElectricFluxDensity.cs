using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 25, Unit.ElectricFluxDensity, Usage.General, Description = "electric density")]
public class DptElectricFluxDensity : Dpt4ByteFloat
{
    private DptElectricFluxDensity()
    {
    }

    public DptElectricFluxDensity(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptElectricFluxDensity(float value)
        : base(value)
    {
    }
}