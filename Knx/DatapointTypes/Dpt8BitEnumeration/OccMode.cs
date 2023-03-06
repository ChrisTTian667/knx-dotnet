using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitEnumeration;

[DatapointEnumValueDescription(3, 255, "Reserved")]
public enum OccMode : byte
{
    Occupied = 0x00,
    StandBy = 0x01,
    NotOccupied = 0x02
}