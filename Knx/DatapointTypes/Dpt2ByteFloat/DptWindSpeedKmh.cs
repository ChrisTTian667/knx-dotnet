using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteFloat;

[DatapointType(9, 28, Unit.WindSpeedKmh, Usage.General)]
public class DptWindSpeedKmh : Dpt2ByteFloat
{
    private DptWindSpeedKmh()
    {
    }

    public DptWindSpeedKmh(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptWindSpeedKmh(double value)
        : base(value)
    {
    }

    [DatapointProperty]
    [Range(0, +670760.96, ErrorMessage = "Windspeed out of Range")]
    public override double Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}