using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteFloat;

[DataLength(16)]
public abstract class Dpt2ByteFloat : DatapointType
{
    protected Dpt2ByteFloat()
    {
    }

    protected Dpt2ByteFloat(byte[] twoBytes) : base(twoBytes, true)
    {
    }

    protected Dpt2ByteFloat(double value)
    {
        Value = value;
    }

    [DatapointProperty]
    public virtual double Value
    {
        get => ToValue(Payload);
        set => Payload = ToBytes(value);
    }

    private static double ToValue(byte[] bytes)
    {
        if (bytes.Length != 2)
            throw new ArgumentOutOfRangeException(nameof(bytes), "Datapoint Type needs exactly 2 bytes of data.");

        var firstByte = bytes[0];
        var secondByte = bytes[1];

        if (firstByte == 0x7F && secondByte == 0xFF) return double.NaN;

        byte[] array = { secondByte, (byte)(firstByte & 0x07) };

        var sign = Convert.ToBoolean(firstByte >> 7);
        var exponent = (firstByte & 0x78) >> 3; // 2,3,4,5 Bit of the first byte
        var mantissa = BitConverter.ToInt16(array, 0);

        if (sign && mantissa == 0) mantissa = 2048;

        return 0.01 * mantissa * Math.Pow(2, exponent) * (sign ? -1 : 1);
    }

    private static byte[] ToBytes(double value)
    {
        var v = value * 100;
        var exponent = 0;

        for (; v < -2048.0f; v /= 2)
            exponent++;
        for (; v > 2047.0f; v /= 2)
            exponent++;

        var mantissa = (short)Math.Abs(Math.Round(v));

        var first = (short)((exponent << 3) | (mantissa >> 8));
        if (value < 0)
            first |= 0x80;

        var second = (byte)mantissa;

        return new ByteArrayBuilder().AddByte((byte)first).AddByte(second).ToByteArray();
    }
}