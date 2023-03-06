using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit;

[DatapointType(1, 17, Unit.Trigger, Usage.General)]
public class DptTrigger : Dpt1Bit
{
    private DptTrigger()
    {
    }

    public DptTrigger(byte[] payload)
        : base(payload)
    {
    }

    public DptTrigger(bool value)
        : base(value)
    {
    }

    [DatapointProperty]
    [BooleanEncoding(UnitEncoding.Trigger, UnitEncoding.Trigger)]
    public override bool Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}