using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitEnumeration;

[DatapointType(20, 11, Usage.FunctionBlock)]
public class DptErrorClassSystem : Dpt8BitEnum<ErrorClassSystem>
{
    private DptErrorClassSystem()
    {
    }

    public DptErrorClassSystem(byte[] payload)
        : base(payload)
    {
    }

    public DptErrorClassSystem(ErrorClassSystem value)
        : base(value)
    {
    }
}