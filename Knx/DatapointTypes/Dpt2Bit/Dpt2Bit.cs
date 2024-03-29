﻿using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2Bit;

[DataLength(2)]
public abstract class Dpt2Bit : Dpt1Bit.Dpt1Bit
{
    protected Dpt2Bit()
    {
    }

    protected Dpt2Bit(byte[] payload)
        : base(payload)
    {
    }

    protected Dpt2Bit(bool value, bool control)
        : base(value)
    {
        Control = control;
        Value = value;
    }

    [DatapointProperty]
    [BooleanEncoding(UnitEncoding.NoControl, UnitEncoding.Control)]
    public bool Control
    {
        get => GetControl(Payload);
        set => Payload = ToBytes(Value, value);
    }

    [DatapointProperty]
    public new virtual bool Value
    {
        get => GetValue(Payload);
        set => Payload = ToBytes(value, Control);
    }

    private static byte[] ToBytes(bool value, bool control)
    {
        return new BitArrayBuilder()
            .Add(false)
            .Add(false)
            .Add(false)
            .Add(false)
            .Add(false)
            .Add(false)
            .Add(control)
            .Add(value)
            .ToBitArray()
            .ToByteArray();
    }

    private static bool GetValue(byte[] bytes)
    {
        if (bytes.Length == 0)
            throw new ArgumentOutOfRangeException("bytes", "Datapoint Type needs at least one byte of data.");

        return Convert.ToBoolean(bytes[0] & 0x01);
    }

    private static bool GetControl(byte[] bytes)
    {
        if (bytes.Length == 0)
            throw new ArgumentOutOfRangeException("bytes", "Datapoint Type needs at least one byte of data.");

        return Convert.ToBoolean(bytes[0] & 0x02);
    }
}