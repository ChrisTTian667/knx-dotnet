using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitEnumeration;

[DatapointType(20, 3, Usage.General)]
public class DptOccMode : Dpt8BitEnum<OccMode>
{
    private DptOccMode()
    {
    }

    public DptOccMode(byte[] payload)
        : base(payload)
    {
    }

    public DptOccMode(OccMode value)
        : base(value)
    {
    }
}