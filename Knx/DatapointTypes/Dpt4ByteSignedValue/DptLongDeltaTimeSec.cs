using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteSignedValue;

[DatapointType(13, 100, Unit.Seconds, Usage.General)]
public class DptLongDeltaTimeSec : Dpt4ByteSignedValue
{
    private DptLongDeltaTimeSec()
    {
    }

    public DptLongDeltaTimeSec(byte[] payload)
        : base(payload)
    {
    }

    public DptLongDeltaTimeSec(TimeSpan timeSpan)
        : base(0)
    {
        Value = timeSpan;
    }

    [DatapointProperty]
    [Range(int.MinValue, int.MaxValue, ErrorMessage = "Timespan must be within -2147483648 s ... 2147483647 seconds.")]
    public new TimeSpan Value
    {
        get => TimeSpan.FromSeconds(base.Value);

        set => base.Value = (int)value.TotalSeconds;
    }
}