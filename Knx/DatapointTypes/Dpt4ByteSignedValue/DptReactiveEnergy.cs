using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteSignedValue;

[DatapointType(13, 12, Unit.VARh, Usage.General)]
public class DptReactiveEnergy : Dpt4ByteSignedValue
{
    private DptReactiveEnergy()
    {
    }

    public DptReactiveEnergy(byte[] payload)
        : base(payload)
    {
    }

    public DptReactiveEnergy(int value)
        : base(value)
    {
    }
}