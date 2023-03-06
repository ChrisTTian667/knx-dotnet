using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit;

[DatapointType(1, 16, Unit.Acknowledge, Usage.General)]
public class DptAcknowledge : Dpt1Bit
{
    private DptAcknowledge()
    {
    }

    public DptAcknowledge(byte[] payload)
        : base(payload)
    {
    }

    public DptAcknowledge(bool value)
        : base(value)
    {
    }

    [DatapointProperty]
    [BooleanEncoding(UnitEncoding.NoAction, UnitEncoding.Acknowledge)]
    public override bool Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}