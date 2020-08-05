using System;
using System.Linq;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DataLength(32)]
    public abstract class Dpt4ByteFloat : DatapointType
    {
        protected Dpt4ByteFloat()
        {
        }

        protected Dpt4ByteFloat(byte[] twoBytes)
        {
            Payload = twoBytes;
        }

        protected Dpt4ByteFloat(float value)
        {
            Value = value;
        }

        [DatapointProperty]
        public virtual float Value
        {
            get
            {
                return BitConverter.ToSingle(Payload.Take(4).Reverse().ToArray(), 0);
            }

            set
            {
                var bytes = BitConverter.GetBytes(value).Take(4);

                if (BitConverter.IsLittleEndian)
                {
                    bytes = bytes.Reverse();
                }

                Payload = bytes.ToArray();
            }
        }
    }
}
