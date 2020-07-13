using System;
using System.Linq;
using System.Reflection;
using Knx.Common;

namespace Knx.DatapointTypes
{
    public abstract class DptEnumeration<T> : DatapointType
    {
        protected DptEnumeration(byte[] payload) : base(payload)
        {
            DoTypeChecks();
        }

        protected DptEnumeration(T value)
        {
            DoTypeChecks();
            Value = value;
        }

        public bool IsValueValid
        {
            get
            {
                return Enum.IsDefined(typeof (T), Payload[0]);
            }
        }

        [DatapointProperty]
        public T Value
        {
            get
            {
                try
                {
                    return (T)Enum.ToObject(typeof(T), Payload[0]);
                }
                catch (Exception exception)
                {
                    throw new Exception(string.Format("Unable to recognize enum value '{0}' in type '{1}'.", Payload[0], typeof(T)), exception);
                }
            }
            set
            {
                try
                {
                    foreach (var enumValue in Enum.GetValues(typeof(T)).Cast<object>().Where(enumValue => enumValue.Equals(value)))
                    {
                        Payload = new[] { (byte)enumValue };
                        return;
                    }
                }
                catch (Exception exception)
                {
                    throw new Exception(string.Format("Unable to find byte interpretation of enum value '{0}' in type '{1}'.", value, typeof(T)), exception);
                }
                finally
                {
                    RaisePropertyChanged(() => Value);
                }

                throw new Exception(string.Format("Unable to find byte interpretation of enum value '{0}'.", value));
            }
        }

        private void DoTypeChecks()
        {
            if (!typeof(T).GetTypeInfo().IsEnum)
                throw new Exception("Generic parameter T must be an enumerable type.");
        }
    }
}