using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteUnsignedValue;

[DatapointType(7, 6, Unit.Minutes, Usage.General)]
public class DptTimePeriodMinutes : DptTimePeriod
{
    private DptTimePeriodMinutes()
    {
    }

    public DptTimePeriodMinutes(byte[] payload)
        : base(payload)
    {
    }

    public DptTimePeriodMinutes(TimeSpan timeSpan)
        : base(timeSpan)
    {
    }

    protected override TimeSpan TimeSpanFromUInt16(ushort value) =>
        TimeSpan.FromMinutes(value);

    protected override ushort UInt16FromTimeSpan(TimeSpan timeSpan)
    {
        var value = timeSpan.TotalMinutes;
        if (value is < 0 or > 65535)
            throw new ArgumentOutOfRangeException(nameof(timeSpan), "Timespan must be within 0 ... 65535 minutes.");

        return (ushort)value;
    }
}
