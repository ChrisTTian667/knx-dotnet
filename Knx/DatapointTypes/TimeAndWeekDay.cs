using System;

namespace Knx.DatapointTypes;

public struct TimeAndWeekDay
{
    public TimeAndWeekDay(TimeSpan time, DayOfWeek? weekday) : this()
    {
        Time = time;
        Weekday = weekday;
    }

    public TimeSpan Time { get; set; }
    public DayOfWeek? Weekday { get; set; }
}