using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit;

[DatapointType(1, 3, Unit.EnableDisable, Usage.General)]
public class DptEnable : Dpt1Bit
{
    private DptEnable()
    {
    }

    public DptEnable(byte[] payload)
        : base(payload)
    {
    }

    public DptEnable(bool value)
        : base(value)
    {
    }

    [DatapointProperty]
    [BooleanEncoding(UnitEncoding.Disable, UnitEncoding.Enable)]
    public override bool Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}