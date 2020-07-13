using System;
using Knx.Common;

namespace Knx.DatapointTypes.Dpt2ByteUnsignedValue
{
    [DatapointType(7, 6, Unit.Minutes, Usage.General)]
    public class DptTimePeriodMinutes : DptTimePeriod
    {
        public DptTimePeriodMinutes(byte[] payload)
            : base(payload)
        {
        }

        public DptTimePeriodMinutes(TimeSpan timeSpan)
            : base(timeSpan)
        {
        }

        protected override TimeSpan TimeSpanFromUInt16(ushort value)
        {
            return TimeSpan.FromMinutes(value);
        }

        protected override ushort UInt16FromTimeSpan(TimeSpan timeSpan)
        {
            var value = timeSpan.TotalMinutes;

            if (value < 0 || value > 65535)
            {
                throw new ArgumentOutOfRangeException("timeSpan", "Timespan must be within 0 ... 65535 minutes.");
            }

            return (UInt16)value;
        }
    }
}