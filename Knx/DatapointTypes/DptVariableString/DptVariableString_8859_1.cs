using System.Text;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.DptVariableString;

[DatapointType(24, 1, Usage.General)]
[DataLength(1, -1)]
public class DptVariableString_8859_1 : DptVariableString
{
    private DptVariableString_8859_1()
    {
    }

    public DptVariableString_8859_1(byte[] payload)
        : base(payload)
    {
    }

    public DptVariableString_8859_1(string character)
        : base(character)
    {
    }

    protected override byte[] ToBytes(string value)
    {
        var byteArray = new byte[value.Length];
        var encodedBytes = Encoding.GetEncoding("iso-8859-1").GetBytes(value);

        for (var i = 0; i < encodedBytes.Length; i++) byteArray[i] = encodedBytes[i];

        return byteArray;
    }

    protected override string ToValue(byte[] bytes)
    {
        return Encoding
            .GetEncoding("iso-8859-1")
            .GetString(bytes, 0, bytes.Length)
            .TrimEnd('\0');
    }
}
