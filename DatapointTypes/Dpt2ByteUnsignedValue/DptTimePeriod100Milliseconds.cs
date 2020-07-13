using System;
using Knx.Common;

namespace Knx.DatapointTypes.Dpt2ByteUnsignedValue
{
    [DatapointType(7, 4, Unit.Milliseconds, Usage.General)]
    public class DptTimePeriod100Milliseconds : DptTimePeriod
    {
        public DptTimePeriod100Milliseconds(byte[] payload)
            : base(payload)
        {
        }

        public DptTimePeriod100Milliseconds(TimeSpan timeSpan)
            : base(timeSpan)
        {
        }

        protected override TimeSpan TimeSpanFromUInt16(ushort value)
        {
            return TimeSpan.FromSeconds(value / 10.0);
        }

        protected override ushort UInt16FromTimeSpan(TimeSpan timeSpan)
        {
            var value = timeSpan.TotalSeconds * 10.0;

            if (value < 0 || value > 65535)
            {
                throw new ArgumentOutOfRangeException("timeSpan", "Timespan must be within 0 ... 6553.5 seconds.");
            }

            return (UInt16)value;
        }
    }
}