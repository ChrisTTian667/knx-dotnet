using System;
using Knx.Common;

namespace Knx.DatapointTypes.Dpt2ByteSignedValue
{
    [DatapointType(8, 2, Unit.Milliseconds, Usage.General)]
    public class DptDeltaTimePeriodMilliseconds : DptDeltaTime
    {
        public DptDeltaTimePeriodMilliseconds(byte[] payload) : base(payload)
        {
        }

        public DptDeltaTimePeriodMilliseconds(TimeSpan timeSpan) : base(timeSpan)
        {
        }

        protected override TimeSpan TimeSpanFromShort(short value)
        {
            return TimeSpan.FromMilliseconds(value);
        }

        protected override short ShortFromTimeSpan(TimeSpan timeSpan)
        {
            var value = timeSpan.TotalMilliseconds;
            if (value < -32768 || value > 32768)
            {
                throw new ArgumentOutOfRangeException("timeSpan", "Timespan must be within -32768 ... 32768 milliseconds.");
            }

            return (short)value;
        }
    }
}