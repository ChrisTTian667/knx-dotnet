using System;
using System.Text;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.DptString;

[DatapointType(16, 1, Usage.General)]
[DataLength(1, 14)]
public class DptString_8859_1 : DptString
{
    private DptString_8859_1()
    {
    }

    public DptString_8859_1(byte[] payload)
        : base(payload)
    {
    }

    public DptString_8859_1(string character)
        : base(character)
    {
    }

    private Encoding Encoding
    {
        get
        {
            var encoding = Encoding.GetEncoding("iso-8859-1");

            if (encoding == null)
                throw new Exception("Unable to retrieve encoding 'iso-8859-1'");

            return encoding;
        }
    }

    protected override byte[] ToBytes(string value)
    {
        var byteArray = new byte[14];
        var encodedBytes = Encoding.GetBytes(value);

        for (var i = 0; i < encodedBytes.Length; i++) byteArray[i] = encodedBytes[i];

        return byteArray;
    }

    protected override string ToValue(byte[] bytes)
    {
        return Encoding.GetString(bytes, 0, bytes.Length).TrimEnd('\0');
    }
}
