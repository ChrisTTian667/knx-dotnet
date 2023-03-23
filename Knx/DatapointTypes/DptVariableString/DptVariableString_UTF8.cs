using System.Text;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.DptVariableString;

[DatapointType(28, 1, Usage.General)]
[DataLength(1, -1)]
public class DptVariableString_UTF8 : DptVariableString
{
    private DptVariableString_UTF8()
    {
    }

    public DptVariableString_UTF8(byte[] payload)
        : base(payload)
    {
    }

    public DptVariableString_UTF8(string character)
        : base(character)
    {
    }

    protected override byte[] ToBytes(string value)
    {
        var byteArray = new byte[value.Length];
        var encodedBytes = Encoding.UTF8.GetBytes(value);

        for (var i = 0; i < encodedBytes.Length; i++)
            byteArray[i] = encodedBytes[i];

        return byteArray;
    }

    protected override string ToValue(byte[] bytes)
    {
        return Encoding.UTF8
            .GetString(bytes, 0, bytes.Length)
            .TrimEnd('\0');
    }
}
