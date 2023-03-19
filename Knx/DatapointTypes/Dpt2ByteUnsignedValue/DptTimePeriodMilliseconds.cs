using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteUnsignedValue;

[DatapointType(7, 2, Unit.Milliseconds, Usage.General)]
public class DptTimePeriodMilliseconds : DptTimePeriod
{
    private DptTimePeriodMilliseconds()
    {
    }

    public DptTimePeriodMilliseconds(byte[] payload) : base(payload)
    {
    }

    public DptTimePeriodMilliseconds(TimeSpan timeSpan) : base(timeSpan)
    {
    }

    protected override TimeSpan TimeSpanFromUInt16(ushort value) =>
        TimeSpan.FromMilliseconds(value);

    protected override ushort UInt16FromTimeSpan(TimeSpan timeSpan)
    {
        var value = timeSpan.TotalMilliseconds;
        if (value is < 0 or > 65535)
            throw new ArgumentOutOfRangeException(nameof(timeSpan), "Timespan must be within 0 ... 65535 milliseconds.");

        return (ushort)value;
    }
}
