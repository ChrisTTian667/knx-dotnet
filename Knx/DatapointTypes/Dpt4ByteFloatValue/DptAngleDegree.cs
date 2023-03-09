using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 7, Unit.Degrees, Usage.General, Description = "angle, degree")]
public class DptAngleDegree : Dpt4ByteFloat
{
    private DptAngleDegree()
    {
    }

    public DptAngleDegree(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptAngleDegree(float value)
        : base(value)
    {
    }
}