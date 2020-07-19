using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteUnsignedValue
{
    [DatapointType(7, 3, Unit.Milliseconds, Usage.General)]
    public class DptTimePeriod10Milliseconds : DptTimePeriod
    {
        public DptTimePeriod10Milliseconds(byte[] payload)
            : base(payload)
        {
        }

        public DptTimePeriod10Milliseconds(TimeSpan timeSpan)
            : base(timeSpan)
        {
        }

        protected override TimeSpan TimeSpanFromUInt16(ushort value)
        {
            return TimeSpan.FromSeconds(value / 100.0);
        }

        protected override ushort UInt16FromTimeSpan(TimeSpan timeSpan)
        {
            var value = timeSpan.TotalSeconds * 100.0;

            if (value < 0 || value > 65535)
            {
                throw new ArgumentOutOfRangeException("timeSpan", "Timespan must be within 0 ... 655.35 seconds.");
            }

            return (UInt16)value;
        }
    }
}