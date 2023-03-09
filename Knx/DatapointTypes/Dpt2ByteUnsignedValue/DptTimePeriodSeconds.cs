using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteUnsignedValue
{
    [DatapointType(7, 5, Unit.Seconds, Usage.General)]
    public class DptTimePeriodSeconds : DptTimePeriod
    {
        public DptTimePeriodSeconds(byte[] payload)
            : base(payload)
        {
        }

        public DptTimePeriodSeconds(TimeSpan timeSpan)
            : base(timeSpan)
        {
        }

        protected override TimeSpan TimeSpanFromUInt16(ushort value)
        {
            return TimeSpan.FromSeconds(value);
        }

        protected override ushort UInt16FromTimeSpan(TimeSpan timeSpan)
        {
            var value = timeSpan.TotalSeconds;

            if (value < 0 || value > 65535)
            {
                throw new ArgumentOutOfRangeException("timeSpan", "Timespan must be within 0 ... 65535 seconds.");
            }

            return (UInt16)value;
        }
    }
}