using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitEnumeration;

[DatapointType(20, 7, Usage.FunctionBlock)]
public class DptAlarmClassType : Dpt8BitEnum<AlarmClassType>
{
    private DptAlarmClassType()
    {
    }

    public DptAlarmClassType(byte[] payload)
        : base(payload)
    {
    }

    public DptAlarmClassType(AlarmClassType value)
        : base(value)
    {
    }
}