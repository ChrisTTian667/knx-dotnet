using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit;

[DatapointType(1, 21, Unit.AndOr, Usage.FunctionBlock)]
public class DptLogicalFunction : Dpt1Bit
{
    private DptLogicalFunction()
    {
    }

    public DptLogicalFunction(byte[] payload)
        : base(payload)
    {
    }

    public DptLogicalFunction(bool value)
        : base(value)
    {
    }

    [DatapointProperty]
    [BooleanEncoding(UnitEncoding.Or, UnitEncoding.And)]
    public override bool Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}