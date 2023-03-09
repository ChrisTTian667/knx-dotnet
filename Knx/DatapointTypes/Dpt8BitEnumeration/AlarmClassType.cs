using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointEnumValueDescription(4, 255, "Reserved")]
    public enum AlarmClassType : byte
    {
        Reserved = 0,
        SimpleAlarm = 1,
        BasicAlarm = 2,
        ExtendedAlarm = 3,
    }
}