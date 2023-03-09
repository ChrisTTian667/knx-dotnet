using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit;

[DatapointType(1, 15, Unit.Reset, Usage.General)]
public class DptReset : Dpt1Bit
{
    private DptReset()
    {
    }

    public DptReset(byte[] payload)
        : base(payload)
    {
    }

    public DptReset(bool value)
        : base(value)
    {
    }

    [DatapointProperty]
    [BooleanEncoding(UnitEncoding.NoAction, UnitEncoding.Reset)]
    public override bool Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}