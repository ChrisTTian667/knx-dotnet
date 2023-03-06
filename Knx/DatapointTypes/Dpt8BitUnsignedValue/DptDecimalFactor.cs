using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitUnsignedValue;

[DatapointType(5, 5, Usage.FunctionBlock)]
public class DptDecimalFactor : Dpt8BitUnsignedValue
{
    private DptDecimalFactor()
    {
    }

    public DptDecimalFactor(byte[] payload)
        : base(payload)
    {
    }

    public DptDecimalFactor(int value)
        : base(value)
    {
    }

    [DatapointProperty]
    [Range(0, 255, ErrorMessage = "Decimal factor must be within 0 and 255")]
    public override int Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}