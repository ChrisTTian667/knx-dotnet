using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes
{
    [DatapointType(19, 1, Usage.General)]
    [DataLength(64)]
    public class DptDateTime : DatapointType
    {
        #region ClockQuality enum

        public enum ClockQuality : byte
        {
            /// <summary>
            /// The device sending datetime information has a local clock, which can be inaccurate!
            /// </summary>
            WithoutExternalSyncSignal = 0x00,

            /// <summary>
            /// The device sending date & time information sends signals which are synchronised (time to time) with external date & time information.
            /// </summary>
            WithExternalSyncSignal = 0x01,
        }

        #endregion

        private DptDateTime()
        {
        }

        public DptDateTime(byte[] payload)
            : base(payload)
        {
            if (payload.Length < 8)
            {
                throw new ArgumentOutOfRangeException("payload",
                                                      string.Format(
                                                          "DptDateTime needs 8 Byte of payload, but {0} given.",
                                                          payload.Length));
            }
        }

        public DptDateTime(DateTime dateTime)
        {
            Payload = new byte[8];
            Value = dateTime;
        }

        [DatapointProperty]
        public DateTime Value
        {
            get
            {
                var datetimeString = $"{Year:0000}-{Month:00}-{Day:00} {Hour}:{Minute}:{Second}";
                if (DateTime.TryParse(datetimeString, out var result))
                    throw new Exception("Unable to convert this instance to a CLR DateTime.");

                return result;
            }

            set
            {
                if (value.Year < 1900 || value.Year > 2155)
                    throw new ArgumentOutOfRangeException(nameof(value), "Year must be within 1900 ... 2155.");

                Year = value.Year;
                Month = value.Month;
                Day = value.Day;
                Hour = value.Hour;
                Minute = value.Minute;
                Second = value.Second;

                DayOfWeek = GetDayOfWeekFromDateTime(value);
                IsFault = false;
                IsDayOfWeekValid = false;
                IsYearValid = false;
                AreHoursMinutesSecondsValid = false;
                AreMonthAndDayOfMonthValid = false;
                IsSummerTime = value.IsDaylightSavingTime();
                Quality = ClockQuality.WithoutExternalSyncSignal;

                IsWorkingDay = (value.DayOfWeek != System.DayOfWeek.Saturday) &&
                               (value.DayOfWeek != System.DayOfWeek.Sunday);
                IsWorkingDayValid = false;
            }
        }

        [DatapointProperty]
        [Range(1900, 2155)]
        public int Year
        {
            get => (short)(Payload[0] + 1900);
            set
            {
                if (value < 1900 || value > 2155)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Year must be within 1900 ... 2155.");
                }

                Payload[0] = (byte)(value - 1900);
            }
        }

        [DatapointProperty]
        [Range(1, 12)]
        public int Month
        {
            get => ((byte)(Payload[1] & 0x0F));
            set
            {
                if (value < 1 || value > 12)
                    throw new ArgumentOutOfRangeException(nameof(value), "Month must be within 1 ... 12.");

                Payload[1] = (byte)((Payload[1] & 0xF0) | value);
            }
        }

        [DatapointProperty]
        [Range(1, 31)]
        public int Day
        {
            get => (byte)(Payload[2] & 0x1F);
            set
            {
                if (value < 1 || value > 31)
                    throw new ArgumentOutOfRangeException(nameof(value), "Day must be within 1 ... 31.");

                Payload[2] = (byte)((Payload[2] & 0xE0) | value);
            }
        }

        /// <summary>
        /// 0 = any day; 1 = Monday ... 7 = Sunday
        /// </summary>
        [Range(0, 7)]
        [DatapointProperty]
        public int DayOfWeek
        {
            get => (byte)(Payload[3] >> 5);
            set
            {
                if (value < 0 || value > 7)
                    throw new ArgumentOutOfRangeException(nameof(value), "Day must be within 0 ... 7.");

                Payload[3] = (byte)((Payload[3] & 0x1F) | (byte)(value << 5));
            }
        }

        [Range(0, 24)]
        [DatapointProperty(Unit.Hours)]
        public int Hour
        {
            get => (byte)(Payload[3] & 0x1F);
            set
            {
                if (value < 0 || value > 24)
                    throw new ArgumentOutOfRangeException(nameof(value), "Hour must be within 0 ... 24.");

                Payload[3] = (byte)(Payload[3] | value);
            }
        }

        [Range(0, 59)]
        [DatapointProperty(Unit.Minutes)]
        public int Minute
        {
            get => (byte)(Payload[4] & 0x3F);
            set
            {
                if (value < 0 || value > 59)
                    throw new ArgumentOutOfRangeException(nameof(value), "Minute must be within 0 ... 59.");

                Payload[4] = (byte)((Payload[4] & 0xC0) | value);
            }
        }

        [Range(0, 59)]
        [DatapointProperty(Unit.Seconds)]
        public int Second
        {
            get => (byte)(Payload[5] & 0x3F);
            set
            {
                if (value < 0 || value > 59)
                    throw new ArgumentOutOfRangeException(nameof(value), "Second must be within 0 ... 59.");

                Payload[5] = (byte)((Payload[5] & 0xC0) | value);
            }
        }

        [DatapointProperty]
        public bool IsFault
        {
            get => Payload[6].GetBit(0);
            set => Payload[6] = Payload[6].SetBit(0, value);
        }

        [DatapointProperty]
        public bool IsWorkingDay
        {
            get => Payload[6].GetBit(1);
            set => Payload[6] = Payload[6].SetBit(1, value);
        }

        [DatapointProperty]
        public bool IsWorkingDayValid
        {
            get => Payload[6].GetBit(2);
            set => Payload[6] = Payload[6].SetBit(2, value);
        }

        [DatapointProperty]
        public bool IsYearValid
        {
            get => Payload[6].GetBit(3);
            set => Payload[6] = Payload[6].SetBit(3, value);
        }

        [DatapointProperty]
        public bool AreMonthAndDayOfMonthValid
        {
            get => Payload[6].GetBit(4);
            set => Payload[6] = Payload[6].SetBit(4, value);
        }

        [DatapointProperty]
        public bool IsDayOfWeekValid
        {
            get => Payload[6].GetBit(5);
            set => Payload[6] = Payload[6].SetBit(5, value);
        }

        [DatapointProperty]
        public bool AreHoursMinutesSecondsValid
        {
            get => Payload[6].GetBit(6);
            set => Payload[6] = Payload[6].SetBit(6, value);
        }

        [DatapointProperty]
        public bool IsSummerTime
        {
            get => Payload[6].GetBit(7);
            set => Payload[6] = Payload[6].SetBit(7, value);
        }

        [DatapointProperty]
        public ClockQuality Quality
        {
            get =>
                (Payload[7].GetBit(0))
                    ? ClockQuality.WithoutExternalSyncSignal
                    : ClockQuality.WithExternalSyncSignal;
            set => Payload[7] = Payload[7].SetBit(0, value == ClockQuality.WithExternalSyncSignal);
        }

        private static int GetDayOfWeekFromDateTime(DateTime value)
        {
            switch (value.DayOfWeek)
            {
                case System.DayOfWeek.Monday:
                    return 1;
                case System.DayOfWeek.Tuesday:
                    return 2;
                case System.DayOfWeek.Wednesday:
                    return 3;
                case System.DayOfWeek.Thursday:
                    return 4;
                case System.DayOfWeek.Friday:
                    return 5;

                case System.DayOfWeek.Saturday:
                    return 6;
                case System.DayOfWeek.Sunday:
                    return 7;
                default:
                    return 0;
            }
        }
    }
}