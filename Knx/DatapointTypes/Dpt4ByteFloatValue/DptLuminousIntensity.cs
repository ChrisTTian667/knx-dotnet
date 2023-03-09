using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 43, Unit.LuminousIntensity, Usage.General, Description = "luminous intensity")]
public class DptLuminousIntensity : Dpt4ByteFloat
{
    private DptLuminousIntensity()
    {
    }

    public DptLuminousIntensity(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptLuminousIntensity(float value)
        : base(value)
    {
    }
}