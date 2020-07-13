using System;
using System.Linq;
using Knx.Common;

namespace Knx.DatapointTypes.Dpt8ByteSignedValue
{
    [DataLength(64)]
    public abstract class Dpt8ByteSignedValue : DatapointType
    {

        protected Dpt8ByteSignedValue(byte[] payload)
            : base(payload)
        {
        }

        protected Dpt8ByteSignedValue(Int64 value)
        {
            Value = value;
        }

        [DatapointProperty]
        [Range(typeof(Int64), "-9223372036854775808", "9223372036854775807", ErrorMessage = "Value must be within -9223372036854775808 ... 9223372036854775807.")]
        public virtual Int64 Value
        {
            get
            {
                var payload = Payload.Take(8).ToArray();

                return BitConverter.ToInt64(payload, 0);
            }

            set
            {
                if (value < Int64.MinValue || value > Int64.MaxValue)
                {
                    throw new ArgumentOutOfRangeException("value", "Value must be within -9223372036854775808 ... 9223372036854775807.");
                }

                var bytes = BitConverter.GetBytes(value);

                Payload = bytes.Take(8).ToArray();
                RaisePropertyChanged(() => Value);
            }
        }
    }
}
