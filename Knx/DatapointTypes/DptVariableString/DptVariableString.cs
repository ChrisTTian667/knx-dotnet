using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.DptVariableString
{
    [DataLength(0, DataLength.Infinite)]
    public abstract class DptVariableString : DatapointType
    {
        protected DptVariableString()
        {
        }

        protected DptVariableString(byte[] payload)
            : base(payload)
        {
            Payload = payload;
        }

        protected DptVariableString(string character)
        {
            Value = character;
        }

        [DatapointProperty]
        public string Value
        {
            get
            {
                return ToValue(Payload);
            }

            set
            {
                Payload = ToBytes(value).Terminate();
                RaisePropertyChanged(() => Value);
            }
        }

        protected abstract byte[] ToBytes(string value);

        protected abstract string ToValue(byte[] bytes);
    }
}