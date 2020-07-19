using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteSignedValue
{
    [DatapointType(8, 7, Unit.Hours, Usage.General)]
    public class DptDeltaTimePeriodHours : DptDeltaTime
    {
        public DptDeltaTimePeriodHours(byte[] payload)
            : base(payload)
        {
        }

        public DptDeltaTimePeriodHours(TimeSpan timeSpan)
            : base(timeSpan)
        {
        }

        protected override TimeSpan TimeSpanFromShort(short value)
        {
            return TimeSpan.FromHours(value);
        }

        protected override short ShortFromTimeSpan(TimeSpan timeSpan)
        {
            var value = timeSpan.TotalHours;

            if (value < -32768 || value > 32767)
            {
                throw new ArgumentOutOfRangeException("timeSpan", "Timespan must be within -32768 ... 32767 hours.");
            }

            return (short)value;
        }
    }
}