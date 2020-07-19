using System;
using System.Linq;
using System.Reflection;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitUnsignedValue
{
    [DataLength(8)]
    public abstract class Dpt8BitUnsignedValue : DatapointType
    {
        protected Dpt8BitUnsignedValue(Byte[] payload)
            : base(payload)
        {
        }

        protected Dpt8BitUnsignedValue(int value)
        {
            Value = value;
        }

        private int GetMaxValue()
        {
            var attribute =
                GetType().GetRuntimeProperty("Value").GetCustomAttributes(typeof(RangeAttribute), true).FirstOrDefault() as RangeAttribute;

            return attribute != null ? Convert.ToInt32(attribute.Maximum) : Byte.MaxValue;
        }

        [DatapointProperty]
        public virtual int Value
        {
            get
            {
                var persitedValue = Payload[0];
                var maxValue = GetMaxValue();

                return (int)Math.Min(Math.Round((maxValue / 255.0) * persitedValue), maxValue);
            }

            set
            {
                var maxValue = GetMaxValue();

                if (value > maxValue)
                {
                    throw new ArgumentOutOfRangeException(string.Format("Property 'Value' of type {0} is out of range.", GetType().Name));
                }

                var computedValue = (byte)((255.0 / maxValue) * value);

                Payload = new[] { computedValue };
                RaisePropertyChanged(() => Value);
            }
        }
    }
}
