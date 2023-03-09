using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointEnumValueDescription(4, 255, "Reserved")]
    public enum Priority : byte
    {
        High = 0x00,
        Medium = 0x01,
        Low = 0x02,
        Void = 0x03,
    }
}