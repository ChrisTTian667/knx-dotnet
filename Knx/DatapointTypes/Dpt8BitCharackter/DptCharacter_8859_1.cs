using System;
using System.Text;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitCharackter;

[DatapointType(4, 2, Usage.General)]
[DataLength(8)]
public class DptCharacter_8859_1 : DatapointType
{
    private DptCharacter_8859_1()
    {
    }

    public DptCharacter_8859_1(byte[] payload)
        : base(payload)
    {
        Payload = payload;
    }

    public DptCharacter_8859_1(char character)
    {
        Value = character;
    }

    [DatapointProperty]
    public char Value
    {
        get => ToValue(Payload);
        set => Payload = ToBytes(value);
    }

    private byte[] ToBytes(char value)
    {
        var encoding = Encoding.GetEncoding("iso-8859-1");

        if (encoding == null) throw new Exception("Unable to retrieve encoding 'iso-8859-1'");

        return encoding.GetBytes(new[] { value }, 0, 1);
    }

    private char ToValue(byte[] bytes)
    {
        var encoding = Encoding.GetEncoding("iso-8859-1");

        if (encoding == null) throw new Exception("Unable to retrieve encoding 'iso-8859-1'");

        var byteString = encoding.GetString(bytes, 0, bytes.Length);

        if (byteString.Length != 1)
        {
            throw new Exception(
                string.Format("Received bytes contains more or less than one charackter. String='{0}'", byteString));
        }

        return byteString[0];
    }
}