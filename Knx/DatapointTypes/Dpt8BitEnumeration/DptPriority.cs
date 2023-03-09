using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitEnumeration;

[DatapointType(20, 4, Usage.General)]
public class DptPriority : Dpt8BitEnum<Priority>
{
    private DptPriority()
    {
    }

    public DptPriority(byte[] payload)
        : base(payload)
    {
    }

    public DptPriority(Priority value)
        : base(value)
    {
    }
}