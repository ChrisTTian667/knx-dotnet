using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitEnumeration;

[DatapointType(20, 8, Usage.System)]
public class DptPSUMode : Dpt8BitEnum<PSUMode>
{
    private DptPSUMode()
    {
    }

    public DptPSUMode(byte[] payload)
        : base(payload)
    {
    }

    public DptPSUMode(PSUMode value)
        : base(value)
    {
    }
}