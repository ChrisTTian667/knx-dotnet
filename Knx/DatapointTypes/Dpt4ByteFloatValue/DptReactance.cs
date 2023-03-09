using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 59, Unit.Reactance, Usage.General, Description = "reactance")]
public class DptReactance : Dpt4ByteFloat
{
    private DptReactance()
    {
    }

    public DptReactance(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptReactance(float value)
        : base(value)
    {
    }
}