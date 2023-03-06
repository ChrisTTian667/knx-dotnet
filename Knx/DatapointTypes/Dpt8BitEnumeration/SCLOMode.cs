using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitEnumeration;

[DatapointEnumValueDescription(3, 255, "Reserved")]
public enum SCLOMode : byte
{
    Autonomous = 0x00,
    Slave = 0x01,
    Master = 0x02
}