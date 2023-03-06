using System;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteUnsignedValue;

public abstract class DptTimePeriod : Dpt2ByteUnsignedValue
{
    protected DptTimePeriod()
    {
    }

    protected DptTimePeriod(byte[] payload) : base(payload)
    {
    }

    protected DptTimePeriod(TimeSpan timeSpan) : base(0)
    {
        base.Value = UInt16FromTimeSpan(timeSpan);
    }

    [DatapointProperty]
    public new TimeSpan Value
    {
        get => TimeSpanFromUInt16(base.Value);

        set => base.Value = UInt16FromTimeSpan(value);
    }

    protected abstract TimeSpan TimeSpanFromUInt16(ushort value);

    protected abstract ushort UInt16FromTimeSpan(TimeSpan timeSpan);
}