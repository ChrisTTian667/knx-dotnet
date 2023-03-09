using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 66, Unit.Stress, Usage.General, Description = "stress")]
public class DptStress : Dpt4ByteFloat
{
    private DptStress()
    {
    }

    public DptStress(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptStress(float value)
        : base(value)
    {
    }
}