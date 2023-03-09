using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitEnumeration;

[DatapointType(20, 1, Usage.FunctionBlock)]
public class DptSCLOMode : Dpt8BitEnum<SCLOMode>
{
    private DptSCLOMode()
    {
    }

    public DptSCLOMode(byte[] payload) : base(payload)
    {
    }

    public DptSCLOMode(SCLOMode value) : base(value)
    {
    }
}