using System;
using System.Text;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitCharackter
{
    [DatapointType(4, 1, Usage.General)]
    [DataLength(8)]
    public class DptCharackterAscii : DatapointType
    {
        private DptCharackterAscii()
        {
        }

        public DptCharackterAscii(byte[] payload)
            : base(payload)
        {
            Payload = payload;
        }

        public DptCharackterAscii(char character)
        {
            Value = character;
        }

        [DatapointProperty]
        public char Value
        {
            get => ToValue(Payload);
            set
            {
                Payload = ToBytes(value);
                RaisePropertyChanged(() => Value);
            }
        }

        private byte[] ToBytes(char value)
        {
            //var ascii = (byte) value;
            //if (ascii <= 0x7F)
            //{
            //    ascii = (byte)'?';
            //}

            //return new byte[1] { ascii };

            return Encoding.UTF8.GetBytes(new[] { value }, 0, 1);
        }

        private char ToValue(byte[] bytes)
        {
            var byteString = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            if (byteString.Length != 1)
            {
                throw new Exception(string.Format("Received bytes contains more or less than one charackter. String='{0}'", byteString));
            }

            return byteString[0];
        }
    }
}
