using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 24, Unit.ElectricFlux, Usage.General, Description = "electric flux")]
public class DptElectricFlux : Dpt4ByteFloat
{
    private DptElectricFlux()
    {
    }

    public DptElectricFlux(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptElectricFlux(float value)
        : base(value)
    {
    }
}