using System;
using System.Collections.Generic;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit;

[DataLength(1)]
public abstract class Dpt1Bit : DatapointType
{
    protected Dpt1Bit()
    {
    }

    protected Dpt1Bit(byte[] payload)
        : base(payload)
    {
    }

    protected Dpt1Bit(bool value)
    {
        Value = value;
    }

    [DatapointProperty]
    public virtual bool Value
    {
        get => ToValue(Payload);
        set => Payload = ToBytes(value);
    }

    private static byte[] ToBytes(bool value)
    {
        return new[] { Convert.ToByte(value) };
    }

    private static bool ToValue(IReadOnlyList<byte> bytes)
    {
        return Convert.ToBoolean(bytes[0]);
    }
}
