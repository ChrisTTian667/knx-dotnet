using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteSignedValue
{
    [DatapointType(8, 3, Unit.Milliseconds, Usage.General)]
    public class DptDeltaTimePeriod10Milliseconds : DptDeltaTime
    {
        private DptDeltaTimePeriod10Milliseconds()
        {
        }

        public DptDeltaTimePeriod10Milliseconds(byte[] payload)
            : base(payload)
        {
        }

        public DptDeltaTimePeriod10Milliseconds(TimeSpan timeSpan)
            : base(timeSpan)
        {
        }

        protected override TimeSpan TimeSpanFromShort(short value)
        {
            return TimeSpan.FromSeconds(value / 100.0);
        }

        protected override short ShortFromTimeSpan(TimeSpan timeSpan)
        {
            var value = timeSpan.TotalSeconds * 100.0;

            if (value < -32768 || value > 32767)
            {
                throw new ArgumentOutOfRangeException("timeSpan", "Timespan must be within -327.68 ... 327.67 seconds.");
            }

            return (short)value;
        }
    }
}