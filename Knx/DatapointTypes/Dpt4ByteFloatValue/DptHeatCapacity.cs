using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 35, Unit.HeatCapacity, Usage.General, Description = "heat capacity")]
public class DptHeatCapacity : Dpt4ByteFloat
{
    private DptHeatCapacity()
    {
    }

    public DptHeatCapacity(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptHeatCapacity(float value)
        : base(value)
    {
    }
}