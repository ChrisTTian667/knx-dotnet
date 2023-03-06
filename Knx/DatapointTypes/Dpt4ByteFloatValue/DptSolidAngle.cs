using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 63, Unit.SolidAngle, Usage.General, Description = "solid angle")]
public class DptSolidAngle : Dpt4ByteFloat
{
    private DptSolidAngle()
    {
    }

    public DptSolidAngle(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptSolidAngle(float value)
        : base(value)
    {
    }
}