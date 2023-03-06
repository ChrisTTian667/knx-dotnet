using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 77, Unit.VolumeFlux, Usage.General, Description = "volume flux")]
public class DptVolumeFlux : Dpt4ByteFloat
{
    private DptVolumeFlux()
    {
    }

    public DptVolumeFlux(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptVolumeFlux(float value)
        : base(value)
    {
    }
}