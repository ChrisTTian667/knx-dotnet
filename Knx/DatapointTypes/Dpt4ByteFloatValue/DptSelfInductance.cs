using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 62, Unit.SelfInductance, Usage.General, Description = "self inductance")]
public class DptSelfInductance : Dpt4ByteFloat
{
    private DptSelfInductance()
    {
    }

    public DptSelfInductance(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptSelfInductance(float value)
        : base(value)
    {
    }
}