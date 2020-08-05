using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteUnsignedValue
{
    [DatapointType(7, 7, Unit.Hours, Usage.General)]
    public class DptTimePeriodHours : DptTimePeriod
    {
        private DptTimePeriodHours()
        {
        }

        public DptTimePeriodHours(byte[] payload)
            : base(payload)
        {
        }

        public DptTimePeriodHours(TimeSpan timeSpan)
            : base(timeSpan)
        {
        }

        protected override TimeSpan TimeSpanFromUInt16(ushort value)
        {
            return TimeSpan.FromHours(value);
        }

        protected override ushort UInt16FromTimeSpan(TimeSpan timeSpan)
        {
            var value = timeSpan.TotalHours;

            if (value < 0 || value > 65535)
            {
                throw new ArgumentOutOfRangeException("timeSpan", "Timespan must be within 0 ... ~7.4 years.");
            }

            return (UInt16)value;
        }
    }
}