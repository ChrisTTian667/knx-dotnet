using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit;

[DatapointType(1, 19, Unit.WindowDoor, Usage.General)]
public class DptWindowDoor : Dpt1Bit
{
    private DptWindowDoor()
    {
    }

    public DptWindowDoor(byte[] payload)
        : base(payload)
    {
    }

    public DptWindowDoor(bool value)
        : base(value)
    {
    }

    [DatapointProperty]
    [BooleanEncoding(UnitEncoding.WindowClosed, UnitEncoding.WindowOpen)]
    public override bool Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}