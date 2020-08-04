using System;
using System.Linq;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8ByteSignedValue
{
    [DataLength(64)]
    public abstract class Dpt8ByteSignedValue : DatapointType
    {
        protected Dpt8ByteSignedValue()
        {
        }

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
        public Int64 Value
        {
            get
            {
                var payload = Payload.Take(8).ToArray();

                return BitConverter.ToInt64(payload, 0);
            }

            set
            {
                var bytes = BitConverter.GetBytes(value);

                Payload = bytes.Take(8).ToArray();
                RaisePropertyChanged(() => Value);
            }
        }
    }
}
