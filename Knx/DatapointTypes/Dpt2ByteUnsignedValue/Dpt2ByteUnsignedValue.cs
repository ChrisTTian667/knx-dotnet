﻿using System;
using System.Linq;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteUnsignedValue
{
    [DataLength(16)]
    public abstract class Dpt2ByteUnsignedValue : DatapointType
    {
        protected Dpt2ByteUnsignedValue(byte[] payload)
            : base(payload)
        {
            Payload = Payload.Take(2).ToArray();
        }

        protected Dpt2ByteUnsignedValue(UInt16 value)
        {
            Value = value;
        }

        [DatapointProperty]
        [Range(0, 65535, ErrorMessage = "Value must be within 0...65535.")]
        public virtual UInt16 Value
        {
            get
            {
                byte[] payload = Payload.Take(2).ToArray();

                return BitConverter.ToUInt16(payload, 0);
            }

            set
            {
                if (value < 0 || value > 65535)
                {
                    throw new ArgumentOutOfRangeException("value", "Value must be within 0 ... 65535.");
                }

                byte[] bytes = BitConverter.GetBytes(value);

                Payload = bytes.Take(2).ToArray();
                RaisePropertyChanged(() => Value);
            }
        }
    }
}