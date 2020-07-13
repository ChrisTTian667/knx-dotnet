using System;
using System.Text;
using Knx.Common;

namespace Knx.DatapointTypes.DptString
{
    [DatapointType(16, 0, Usage.General)]
    [DataLength(1, 14)]
    public class DptStringAscii : DptString
    {
        public DptStringAscii(byte[] payload) : base(payload)
        {
        }

        public DptStringAscii(string character) : base(character)
        {
        }

        protected override byte[] ToBytes(string value)
        {
            var result = new byte[14];
            var content = new ASCIIEncoding().GetBytes(value);

            for (var i = 0; i < Math.Min(content.Length, 13); i++)
                result[i] = content[i];

            return result;
        }

        protected override string ToValue(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes, 0 , bytes.Length).TrimEnd('\0');
        }
    }
}
