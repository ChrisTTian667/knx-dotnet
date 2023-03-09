using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit;

[DatapointType(1, 12, Unit.InvertedNotInverted, Usage.FunctionBlock)]
public class DptInvert : Dpt1Bit
{
    private DptInvert()
    {
    }

    public DptInvert(byte[] payload)
        : base(payload)
    {
    }

    public DptInvert(bool value)
        : base(value)
    {
    }

    [DatapointProperty]
    [BooleanEncoding(UnitEncoding.NotInverted, UnitEncoding.Inverted)]
    public override bool Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}