using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit;

[DatapointType(1, 9, Unit.OpenClose, Usage.General)]
public class DptOpenClose : Dpt1Bit
{
    private DptOpenClose()
    {
    }

    public DptOpenClose(byte[] payload)
        : base(payload)
    {
    }

    public DptOpenClose(bool value)
        : base(value)
    {
    }

    [DatapointProperty]
    [BooleanEncoding(UnitEncoding.Close, UnitEncoding.Open)]
    public override bool Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}