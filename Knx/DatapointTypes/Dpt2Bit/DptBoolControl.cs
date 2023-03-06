using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2Bit;

[DatapointType(2, 2, Unit.TrueFalse, Usage.General)]
public class DptBoolControl : Dpt2Bit
{
    private DptBoolControl()
    {
    }

    public DptBoolControl(byte[] payload)
        : base(payload)
    {
    }

    public DptBoolControl(bool value, bool control)
        : base(value, control)
    {
    }

    [DatapointProperty]
    [BooleanEncoding(UnitEncoding.False, UnitEncoding.True)]
    public override bool Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}