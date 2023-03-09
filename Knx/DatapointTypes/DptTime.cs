using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes
{
    [DatapointType(10, 1, Usage.General)]
    [DataLength(24)]
    public class DptTime : DatapointType
    {
        #region Constructor / Destructor

        public DptTime()
        {
        }

        public DptTime(TimeSpan time, DayOfWeek? dayOfWeek)
        {
            Value = time;
            DayOfWeek = dayOfWeek;
        }

        public DptTime(Byte[] bytes)
        {
            Payload = bytes;
        }

        #endregion

        #region Properties

        [DatapointProperty]
        public TimeSpan Value
        {
            get { return ToValue(Payload).Time; }

            set { Payload = ToBytes(value, DayOfWeek); }
        }

        [DatapointProperty]
        public DayOfWeek? DayOfWeek { get; set; }

        #endregion

        public static byte[] ToBytes(TimeSpan time, DayOfWeek? dayOfWeek)
        {
            var day = ((byte) GetDayOfWeek(dayOfWeek)) << 5;
            var hour = (byte) time.Hours;
            var firstByte = (byte) (day | hour);
            return new[] {firstByte, (byte) time.Minutes, (byte) time.Seconds};
        }

        private static DayOfWeek? GetDayOfWeek(int day)
        {
            if (day == 0)
            {
                return null;
            }

            if (day == 7)
            {
                day = 0;
            }

            return (DayOfWeek) (day);
        }

        private static int GetDayOfWeek(DayOfWeek? day)
        {
            if (day == null)
            {
                return 0;
            }

            if (day == System.DayOfWeek.Sunday)
            {
                return 7;
            }

            return (int) day;
        }

        private static TimeAndWeekDay ToValue(byte[] bytes)
        {
            if (bytes.Length != 3)
            {
                throw new ArgumentOutOfRangeException("bytes", "Time value must be 3 bytes long.");
            }

            var firstByte = bytes[0];
            var secondbyte = bytes[1];
            var thirdbyte = bytes[2];

            var day = firstByte >> 5;
            var hour = firstByte & 0x1F;
            var minutes = secondbyte & 0x3F;
            var seconds = thirdbyte & 0x3F;

            return new TimeAndWeekDay(new TimeSpan(hour, minutes, seconds), GetDayOfWeek(day));
        }
    }
}