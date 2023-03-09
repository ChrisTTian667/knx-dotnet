using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 37, Unit.HeatQuantity, Usage.General, Description = "heat, quantity of")]
public class DptHeatQuantity : Dpt4ByteFloat
{
    private DptHeatQuantity()
    {
    }

    public DptHeatQuantity(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptHeatQuantity(float value)
        : base(value)
    {
    }
}