using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteSignedValue
{
    [DatapointType(8, 5, Unit.Seconds, Usage.General)]
    public class DptDeltaTimePeriodSeconds : DptDeltaTime
    {
        public DptDeltaTimePeriodSeconds(byte[] payload)
            : base(payload)
        {
        }

        public DptDeltaTimePeriodSeconds(TimeSpan timeSpan)
            : base(timeSpan)
        {
        }

        protected override TimeSpan TimeSpanFromShort(short value)
        {
            return TimeSpan.FromSeconds(value);
        }

        protected override short ShortFromTimeSpan(TimeSpan timeSpan)
        {
            var value = timeSpan.TotalSeconds;

            if (value < -32768 || value > 32767)
            {
                throw new ArgumentOutOfRangeException("timeSpan", "Timespan must be within -32768 ... 32767 seconds.");
            }

            return (short)value;
        }
    }
}