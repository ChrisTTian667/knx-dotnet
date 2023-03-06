using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteSignedValue;

[DatapointType(8, 4, Unit.Milliseconds, Usage.General)]
public class DptDeltaTimePeriod100Milliseconds : DptDeltaTime
{
    private DptDeltaTimePeriod100Milliseconds()
    {
    }

    public DptDeltaTimePeriod100Milliseconds(byte[] payload)
        : base(payload)
    {
    }

    public DptDeltaTimePeriod100Milliseconds(TimeSpan timeSpan)
        : base(timeSpan)
    {
    }

    protected override TimeSpan TimeSpanFromShort(short value)
    {
        return TimeSpan.FromSeconds(value / 10.0);
    }

    protected override short ShortFromTimeSpan(TimeSpan timeSpan)
    {
        var value = timeSpan.TotalSeconds * 10.0;

        if (value < -32768 || value > 32767)
            throw new ArgumentOutOfRangeException("timeSpan", "Timespan must be within -3276.8 ... 3276.7 seconds.");

        return (short)value;
    }
}