using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteFloat;

[DatapointType(9, 26, Unit.RainAmount, Usage.General)]
public class DptRainAmount : Dpt2ByteFloat
{
    private DptRainAmount()
    {
    }

    public DptRainAmount(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptRainAmount(double value)
        : base(value)
    {
    }

    [DatapointProperty]
    [Range(-671088.64, +670760.96, ErrorMessage = "Rain amount out of Range")]
    public override double Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}