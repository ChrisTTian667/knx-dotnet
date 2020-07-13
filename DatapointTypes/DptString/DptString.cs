using System;
using Knx.Common;

namespace Knx.DatapointTypes.DptString
{
    [DataLength(0, 112)]
    public abstract class DptString : DatapointType
    {
        protected DptString(byte[] payload)
            : base(payload)
        {
            Payload = payload;
        }

        protected DptString(string character)
        {
            Value = character;
        }

        [DatapointProperty]
        [StringLength(14, ErrorMessage = "String must not exceed a length of 14 charackters.")]
        public string Value
        {
            get
            {
                return ToValue(Payload);
            }

            set
            {
                if (value.Length > 14)
                {
                    throw new Exception("String must not exceed a length of 14 charackters.");
                }

                Payload = ToBytes(value);
                RaisePropertyChanged(() => Value);
            }
        }

        protected abstract byte[] ToBytes(string value);

        protected abstract string ToValue(byte[] bytes);
    }
}