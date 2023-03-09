using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 69, Unit.Kelvin, Usage.General, Description = "(absolute) temperature")]
public class DptAbsoluteTemperature : Dpt4ByteFloat
{
    private DptAbsoluteTemperature()
    {
    }

    public DptAbsoluteTemperature(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptAbsoluteTemperature(float value)
        : base(value)
    {
    }
}