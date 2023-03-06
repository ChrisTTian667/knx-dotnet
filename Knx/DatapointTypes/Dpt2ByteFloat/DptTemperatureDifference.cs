using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteFloat;

[DatapointType(9, 2, Unit.TemperatureDifference, Usage.General)]
public class DptTemperatureDifference : Dpt2ByteFloat
{
    private DptTemperatureDifference()
    {
    }

    public DptTemperatureDifference(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptTemperatureDifference(double value)
        : base(value)
    {
    }

    [DatapointProperty]
    [Range(-670760, +670760, ErrorMessage = "Temperature differnce out of Range")]
    public override double Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}