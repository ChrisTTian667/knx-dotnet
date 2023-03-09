using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit;

[DatapointType(1, 7, Unit.IncreaseDecrease, Usage.FunctionBlock)]
public class DptStep : Dpt1Bit
{
    private DptStep()
    {
    }

    public DptStep(byte[] payload)
        : base(payload)
    {
    }

    public DptStep(bool value)
        : base(value)
    {
    }

    [DatapointProperty]
    [BooleanEncoding(UnitEncoding.Decrease, UnitEncoding.Increase)]
    public override bool Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}