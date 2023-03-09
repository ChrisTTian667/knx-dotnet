using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2Bit;

[DatapointType(2, 6, Unit.HighLow, Usage.FunctionBlock)]
public class DptBinaryValueControl : Dpt2Bit
{
    private DptBinaryValueControl()
    {
    }

    public DptBinaryValueControl(byte[] payload)
        : base(payload)
    {
    }

    public DptBinaryValueControl(bool value, bool control)
        : base(value, control)
    {
    }

    [DatapointProperty]
    [BooleanEncoding(UnitEncoding.Low, UnitEncoding.High)]
    public override bool Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}