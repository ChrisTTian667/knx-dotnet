using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 75, Unit.Torque, Usage.General, Description = "torque")]
public class DptTorque : Dpt4ByteFloat
{
    private DptTorque()
    {
    }

    public DptTorque(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptTorque(float value)
        : base(value)
    {
    }
}