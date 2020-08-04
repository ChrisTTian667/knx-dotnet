using System;
using System.Linq;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteSignedValue
{
    [DataLength(32)]
    public abstract class Dpt4ByteSignedValue : DatapointType
    {
        protected Dpt4ByteSignedValue()
        {
        }
        
        protected Dpt4ByteSignedValue(byte[] payload)
            : base(payload)
        {
        }

        protected Dpt4ByteSignedValue(int value)
        {
            Value = value;
        }

        [DatapointProperty]
        [Range(int.MinValue, int.MaxValue, ErrorMessage = "Value must be within -2147483648 ... 2147483647.")]
        public virtual int Value
        {
            get
            {
                var payload = Payload.Take(4).ToArray();

                return BitConverter.ToInt32(payload, 0);
            }

            set
            {
                if (value < int.MinValue || value > int.MaxValue)
                {
                    throw new ArgumentOutOfRangeException("value", "Value must be within -2147483648 ... 2147483647.");
                }

                var bytes = BitConverter.GetBytes(value);

                Payload = bytes.Take(4).ToArray();
                RaisePropertyChanged(() => Value);
            }
        }
    }
}
