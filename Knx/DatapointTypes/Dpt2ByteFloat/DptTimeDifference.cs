using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteFloat;

[DatapointType(9, 10, Unit.TimeDifferenceS, Usage.General)]
[DatapointType(9, 11, Unit.TimeDifferenceMS, Usage.General)]
public class DptTimeDifference : Dpt2ByteFloat
{
    private DptTimeDifference()
    {
    }

    public DptTimeDifference(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptTimeDifference(double value)
        : base(value)
    {
    }

    [DatapointProperty]
    [Range(-670760, +670760, ErrorMessage = "Time difference out of Range")]
    public override double Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}