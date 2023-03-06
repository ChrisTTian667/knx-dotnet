using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteFloat;

[DatapointType(9, 25, Unit.VolumeFlow, Usage.FunctionBlock)]
public class DptVolumeFlow : Dpt2ByteFloat
{
    private DptVolumeFlow()
    {
    }

    public DptVolumeFlow(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptVolumeFlow(double value)
        : base(value)
    {
    }

    [DatapointProperty]
    [Range(-670760, +670760, ErrorMessage = "Volume flow out of Range")]
    public override double Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}