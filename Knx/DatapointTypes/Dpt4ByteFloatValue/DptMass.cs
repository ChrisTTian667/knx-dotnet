using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 51, Unit.Kilogram, Usage.General, Description = "mass")]
public class DptMass : Dpt4ByteFloat
{
    private DptMass()
    {
    }

    public DptMass(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptMass(float value)
        : base(value)
    {
    }
}