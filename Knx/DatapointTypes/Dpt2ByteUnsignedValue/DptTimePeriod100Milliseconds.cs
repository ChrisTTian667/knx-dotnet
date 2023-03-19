using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteUnsignedValue;

[DatapointType(7, 4, Unit.Milliseconds, Usage.General)]
public class DptTimePeriod100Milliseconds : DptTimePeriod
{
    private DptTimePeriod100Milliseconds()
    {
    }

    public DptTimePeriod100Milliseconds(byte[] payload)
        : base(payload)
    {
    }

    public DptTimePeriod100Milliseconds(TimeSpan timeSpan)
        : base(timeSpan)
    {
    }

    protected override TimeSpan TimeSpanFromUInt16(ushort value) =>
        TimeSpan.FromSeconds(value / 10.0);

    protected override ushort UInt16FromTimeSpan(TimeSpan timeSpan)
    {
        var value = timeSpan.TotalSeconds * 10.0;

        if (value is < 0 or > 65535)
            throw new ArgumentOutOfRangeException(nameof(timeSpan), "Timespan must be within 0 ... 6553.5 seconds.");

        return (ushort)value;
    }
}
