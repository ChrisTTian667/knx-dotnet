using System;
using Knx.Common;

namespace Knx.DatapointTypes.Dpt2ByteSignedValue
{
    [DatapointType(8, 6, Unit.Minutes, Usage.General)]
    public class DptDeltaTimePeriodMinutes : DptDeltaTime
    {
        public DptDeltaTimePeriodMinutes(byte[] payload)
            : base(payload)
        {
        }

        public DptDeltaTimePeriodMinutes(TimeSpan timeSpan)
            : base(timeSpan)
        {
        }

        protected override TimeSpan TimeSpanFromShort(short value)
        {
            return TimeSpan.FromMinutes(value);
        }

        protected override short ShortFromTimeSpan(TimeSpan timeSpan)
        {
            var value = timeSpan.TotalMinutes;

            if (value < -32768 || value > 32767)
            {
                throw new ArgumentOutOfRangeException("timeSpan", "Timespan must be within -32768 ... 32767 minutes.");
            }

            return (short)value;
        }
    }
}