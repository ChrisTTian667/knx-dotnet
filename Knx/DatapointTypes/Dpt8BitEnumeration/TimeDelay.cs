using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointEnumValueDescription(26, 255, "Reserved")]
    public enum TimeDelay : byte
    {
        NotActive = 0,
        OneSecond = 1,
        TwoSeconds = 2,
        ThreeSeconds = 3,
        FiveSeconds = 4,
        TenSeconds = 5,
        FiveteenSeconds = 6,
        TwentySeconds = 7,
        ThirtySeconds = 8,
        FourtyFiveSeconds = 9,
        OneMinute = 10,
        OneAndAQuarterMinute = 11,
        OndAndAHalfMinute = 12,
        TwoMinutes = 13,
        TwoAndAHalfMinute = 14,
        ThreeMinutes = 15,
        FiveMinutes = 16,
        FiveteenMinutes = 17,
        TwentyMinutes = 18,
        ThirtyMinutes = 19,
        OneHour = 20,
        TwoHours = 21,
        ThreeHours = 22,
        FiveHours = 23,
        TwelveHours = 24,
        TwentyFourHours = 25,
    }
}