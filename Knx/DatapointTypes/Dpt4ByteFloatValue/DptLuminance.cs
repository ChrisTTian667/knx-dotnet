using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 41, Unit.Luminance, Usage.General, Description = "luminance")]
public class DptLuminance : Dpt4ByteFloat
{
    private DptLuminance()
    {
    }

    public DptLuminance(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptLuminance(float value)
        : base(value)
    {
    }
}