using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 32, Unit.Force, Usage.General, Description = "force")]
public class DptForce : Dpt4ByteFloat
{
    private DptForce()
    {
    }

    public DptForce(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptForce(float value)
        : base(value)
    {
    }
}