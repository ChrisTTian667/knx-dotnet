using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteFloat;

[DatapointType(9, 20, Unit.Voltage, Usage.General)]
public class DptVoltage : Dpt2ByteFloat
{
    private DptVoltage()
    {
    }

    public DptVoltage(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptVoltage(double value)
        : base(value)
    {
    }

    [DatapointProperty]
    [Range(-670760, +670760, ErrorMessage = "Voltage out of Range")]
    public override double Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}