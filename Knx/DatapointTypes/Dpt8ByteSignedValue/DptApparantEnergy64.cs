using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8ByteSignedValue;

[DatapointType(29, 11, Unit.VAh, Usage.General)]
public class DptApparantEnergy64 : Dpt8ByteSignedValue
{
    private DptApparantEnergy64()
    {
    }

    public DptApparantEnergy64(byte[] payload)
        : base(payload)
    {
    }

    public DptApparantEnergy64(long value)
        : base(value)
    {
    }
}