using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 76, Unit.CubicMeter, Usage.General, Description = "volume")]
public class DptVolume : Dpt4ByteFloat
{
    private DptVolume()
    {
    }

    public DptVolume(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptVolume(float value)
        : base(value)
    {
    }
}