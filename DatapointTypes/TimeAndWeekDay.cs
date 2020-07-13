using System;

namespace Knx.DatapointTypes
{
    public struct TimeAndWeekDay
    {
        #region Constructor / Destructor

        public TimeAndWeekDay(TimeSpan time, DayOfWeek? weekday) : this()
        {
            Time = time;
            Weekday = weekday;
        }

        #endregion

        #region Properties

        public TimeSpan Time { get; set; }
        public DayOfWeek? Weekday { get; set; }

        #endregion
    }
}