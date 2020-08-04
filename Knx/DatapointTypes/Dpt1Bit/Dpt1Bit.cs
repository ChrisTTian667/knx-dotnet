using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit
{
    [DataLength(1)]
    public abstract class Dpt1Bit : DatapointType
    {
        protected Dpt1Bit()
        {
        }
        
        protected Dpt1Bit(Byte[] payload)
            : base(payload)
        {
        }

        protected Dpt1Bit(Boolean value)
        {
            Value = value;
        }

        [DatapointProperty]
        public virtual Boolean Value
        {
            get => ToValue(Payload);
            set
            {
                Payload = ToBytes(value);
                RaisePropertyChanged(() => Value);
            }
        }

        private static byte[] ToBytes(bool value)
        {
            return new byte[1] { Convert.ToByte(value) };
        }

        private static bool ToValue(byte[] bytes)
        {
            return Convert.ToBoolean(bytes[0]);
        }
    }
}