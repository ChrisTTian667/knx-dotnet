using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes;

[DatapointType(11, 1, Usage.General)]
[DataLength(24)]
public class DptDate : DatapointType
{
    public DptDate()
    {
    }

    public DptDate(byte[] bytes)
    {
        Payload = bytes;
    }

    public DptDate(DateTime date)
    {
        Value = date;
    }

    [DatapointProperty]
    public DateTime Value
    {
        get => ToValue(Payload);

        set => Payload = ToBytes(value);
    }

    private static DateTime ToValue(byte[] bytes)
    {
        if (bytes.Length != 3) throw new ArgumentOutOfRangeException("bytes", "Date value must be 3 bytes long.");

        return new DateTime(bytes[0] & 0x1F, bytes[1] & 0x0F, bytes[2] & 0x7F);
    }

    public static byte[] ToBytes(DateTime date)
    {
        return new[] { (byte)(date.Day & 0x1F), (byte)(date.Month & 0x0F), (byte)(date.Year & 0x7F) };
    }
}