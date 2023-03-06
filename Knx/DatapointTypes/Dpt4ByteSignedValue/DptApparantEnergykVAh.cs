using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteSignedValue;

[DatapointType(13, 14, Unit.kVAh, Usage.General)]
public class DptApparantEnergykVAh : Dpt4ByteSignedValue
{
    private DptApparantEnergykVAh()
    {
    }

    public DptApparantEnergykVAh(byte[] payload)
        : base(payload)
    {
    }

    public DptApparantEnergykVAh(int value)
        : base(value)
    {
    }
}