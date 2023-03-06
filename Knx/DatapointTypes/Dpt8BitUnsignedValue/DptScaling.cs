using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitUnsignedValue;

[DatapointType(5, 1, Unit.Percentage, Usage.General)]
public class DptScaling : Dpt8BitUnsignedValue
{
    private DptScaling()
    {
    }

    public DptScaling(byte[] payload)
        : base(payload)
    {
    }

    public DptScaling(int value)
        : base(value)
    {
    }

    [DatapointProperty]
    [Range(0, 100, ErrorMessage = "Scaling must be within 0 and 100")]
    public override int Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}