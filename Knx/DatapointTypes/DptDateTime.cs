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
                DateTime result;
                string datetimeString = string.Format("{0:0000}-{1:00}-{2:00} {3}:{4}:{5}", Year, Month, Day, Hour,
                                                      Minute, Second);

                if (DateTime.TryParse(datetimeString, out result))
                {
                    throw new Exception("Unable to convert this instance to a CLR DateTime.");
                }


                return result;
            }

            set
            {
                if (value.Year < 1900 || value.Year > 2155)
                {
                    throw new ArgumentOutOfRangeException("value", "Year must be within 1900 ... 2155.");
                }

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

                RaisePropertyChanged(() => Value);
            }
        }

        [DatapointProperty]
        [Range(1900, 2155)]
        public int Year
        {
            get { return (short)(Payload[0] + 1900); }

            set
            {
                if (value < 1900 || value > 2155)
                {
                    throw new ArgumentOutOfRangeException("value", "Year must be within 1900 ... 2155.");
                }

                Payload[0] = (byte)(value - 1900);
                RaisePropertyChanged(() => Year);
            }
        }

        [DatapointProperty]
        [Range(1, 12)]
        public int Month
        {
            get { return ((byte)(Payload[1] & 0x0F)); }

            set
            {
                if (value < 1 || value > 12)
                {
                    throw new ArgumentOutOfRangeException("value", "Month must be within 1 ... 12.");
                }

                Payload[1] = (byte)((Payload[1] & 0xF0) | value);
                RaisePropertyChanged(() => Month);
            }
        }

        [DatapointProperty]
        [Range(1, 31)]
        public int Day
        {
            get { return (byte)(Payload[2] & 0x1F); }

            set
            {
                if (value < 1 || value > 31)
                {
                    throw new ArgumentOutOfRangeException("value", "Day must be within 1 ... 31.");
                }

                Payload[2] = (byte)((Payload[2] & 0xE0) | value);
                RaisePropertyChanged(() => Day);
            }
        }

        /// <summary>
        /// 0 = any day; 1 = Monday ... 7 = Sunday
        /// </summary>
        [Range(0, 7)]
        [DatapointProperty]
        public int DayOfWeek
        {
            get { return (byte)(Payload[3] >> 5); }

            set
            {
                if (value < 0 || value > 7)
                {
                    throw new ArgumentOutOfRangeException("value", "Day must be within 0 ... 7.");
                }

                Payload[3] = (byte)((Payload[3] & 0x1F) | (byte)(value << 5));
                RaisePropertyChanged(() => DayOfWeek);
            }
        }

        [Range(0, 24)]
        [DatapointProperty(Unit.Hours)]
        public int Hour
        {
            get { return (byte)(Payload[3] & 0x1F); }

            set
            {
                if (value < 0 || value > 24)
                {
                    throw new ArgumentOutOfRangeException("value", "Hour must be within 0 ... 24.");
                }

                Payload[3] = (byte)(Payload[3] | value);
                RaisePropertyChanged(() => Hour);
            }
        }

        [Range(0, 59)]
        [DatapointProperty(Unit.Minutes)]
        public int Minute
        {
            get { return (byte)(Payload[4] & 0x3F); }

            set
            {
                if (value < 0 || value > 59)
                {
                    throw new ArgumentOutOfRangeException("value", "Minute must be within 0 ... 59.");
                }

                Payload[4] = (byte)((Payload[4] & 0xC0) | value);
                RaisePropertyChanged(() => Minute);
            }
        }

        [Range(0, 59)]
        [DatapointProperty(Unit.Seconds)]
        public int Second
        {
            get { return (byte)(Payload[5] & 0x3F); }

            set
            {
                if (value < 0 || value > 59)
                {
                    throw new ArgumentOutOfRangeException("value", "Second must be within 0 ... 59.");
                }

                Payload[5] = (byte)((Payload[5] & 0xC0) | value);
                RaisePropertyChanged(() => Second);
            }
        }

        [DatapointProperty]
        public bool IsFault
        {
            get { return Payload[6].GetBit(0); }

            set
            {
                Payload[6] = Payload[6].SetBit(0, value);
                RaisePropertyChanged(() => IsFault);
            }
        }

        [DatapointProperty]
        public bool IsWorkingDay
        {
            get { return Payload[6].GetBit(1); }
            set
            {
                Payload[6] = Payload[6].SetBit(1, value);
                RaisePropertyChanged(() => IsWorkingDay);
            }
        }

        [DatapointProperty]
        public bool IsWorkingDayValid
        {
            get { return Payload[6].GetBit(2); }
            set
            {
                Payload[6] = Payload[6].SetBit(2, value);
                RaisePropertyChanged(() => IsWorkingDayValid);
            }
        }

        [DatapointProperty]
        public bool IsYearValid
        {
            get { return Payload[6].GetBit(3); }
            set
            {
                Payload[6] = Payload[6].SetBit(3, value);
                RaisePropertyChanged(() => IsYearValid);
            }
        }

        [DatapointProperty]
        public bool AreMonthAndDayOfMonthValid
        {
            get { return Payload[6].GetBit(4); }
            set
            {
                Payload[6] = Payload[6].SetBit(4, value);
                RaisePropertyChanged(() => AreMonthAndDayOfMonthValid);
            }
        }

        [DatapointProperty]
        public bool IsDayOfWeekValid
        {
            get { return Payload[6].GetBit(5); }
            set
            {
                Payload[6] = Payload[6].SetBit(5, value);
                RaisePropertyChanged(() => IsDayOfWeekValid);
            }
        }

        [DatapointProperty]
        public bool AreHoursMinutesSecondsValid
        {
            get { return Payload[6].GetBit(6); }
            set
            {
                Payload[6] = Payload[6].SetBit(6, value);
                RaisePropertyChanged(() => AreHoursMinutesSecondsValid);
            }
        }

        [DatapointProperty]
        public bool IsSummerTime
        {
            get
            {
                return Payload[6].GetBit(7);
            }

            set
            {
                Payload[6] = Payload[6].SetBit(7, value);
                RaisePropertyChanged(() => IsSummerTime);
            }
        }

        [DatapointProperty]
        public ClockQuality Quality
        {
            get
            {
                return (Payload[7].GetBit(0))
                           ? ClockQuality.WithoutExternalSyncSignal
                           : ClockQuality.WithExternalSyncSignal;
            }
            set
            {
                Payload[7] = Payload[7].SetBit(0, value == ClockQuality.WithExternalSyncSignal);
                RaisePropertyChanged(() => Quality);
            }
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