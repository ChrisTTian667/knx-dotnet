using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteFloat;

[DatapointType(9, 6, Unit.Pa, Usage.General)]
public class DptAirPressure : Dpt2ByteFloat
{
    private DptAirPressure()
    {
    }

    public DptAirPressure(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptAirPressure(double value)
        : base(value)
    {
    }

    [DatapointProperty]
    [Range(0, +670760, ErrorMessage = "AirPressure out of Range")]
    public override double Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}