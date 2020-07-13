using Knx.Common;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointEnumValueDescription(3, 16, "Reserved")]
    [DatapointEnumValueDescription(17, 255, "Manufacturer specific")]
    public enum ApplicationMode : byte
    {
        Normal = 0x00,
        PresenceSimulation = 0x01,
        NightRound = 0x02,
    }
}