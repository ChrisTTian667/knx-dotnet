using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 3, Unit.Activity, Usage.General, Description = "activity (radioactive)")]
public class DptActivity : Dpt4ByteFloat
{
    private DptActivity()
    {
    }

    public DptActivity(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptActivity(float value)
        : base(value)
    {
    }
}