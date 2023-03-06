using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteSignedValue;

[DatapointType(13, 10, Unit.Wh, Usage.General)]
public class DptActiveEnergy : Dpt4ByteSignedValue
{
    private DptActiveEnergy()
    {
    }

    public DptActiveEnergy(byte[] payload)
        : base(payload)
    {
    }

    public DptActiveEnergy(int value)
        : base(value)
    {
    }
}