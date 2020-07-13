using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Knx.DatapointTypes;

namespace Knx
{
    /// <summary>
    /// Wrapper for Enumerations
    /// </summary>
    public class Enumeration
    {
        private readonly Type _enumType;
        private string _enumerationName;

        public Enumeration(Type enumerationType)
        {
            if (enumerationType == null)
                throw new ArgumentNullException("enumerationType", "Value cannot be null.");

            if (typeof(DptEnumeration<>).IsAssignableFrom(enumerationType) || enumerationType.Name.StartsWith("Nullable"))
            {
                if (!enumerationType.GenericTypeArguments.Any())
                    throw new ArgumentException("Enumeration type does not contain generic parameter.");
                enumerationType = enumerationType.GenericTypeArguments.First();
            }

            if (!enumerationType.GetTypeInfo().IsEnum)
                throw new ArgumentException("Enumeration must be an enumerable type.", "enumerationType");

            _enumType = enumerationType;
            _enumerationName = GetEnumNames().First();
        }

        private IEnumerable<string> GetEnumNames()
        {
            return Enum.GetNames(_enumType);
        }
        
        private IEnumerable<object> GetEnumValues()
        {
            return Enum.GetValues(_enumType).Cast<object>();
        }

        public IEnumerable<string> Names => GetEnumNames();

        public IEnumerable<object> Values => GetEnumValues();


        /// <summary>
        /// Gets or sets the enumeration value as string.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Name
        {
            get
            {
                // this makes sure, that the return values is always capitalized correctly.
                return Names.First(v => v.Equals(_enumerationName, StringComparison.OrdinalIgnoreCase));
            }

            set
            {
                // only allow setting the value, if it's an correct string representation of one of the supported Values.
                if (!Names.Contains(value, StringComparer.CurrentCultureIgnoreCase))
                {
                    throw new InvalidOperationException(string.Format("Incorrect Name. Possible names: {0}", String.Join(", ", this.Names)));
                }

                _enumerationName = value;
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value
        {
            get
            {
                // returns the value of the current set 'name' (or null)
                return Values.FirstOrDefault(value => Enum.GetName(_enumType, value).Equals(_enumerationName, StringComparison.OrdinalIgnoreCase));
            }
            set
            {
                Name = Enum.GetName(_enumType, value);
            }
        }

        public static implicit operator Enumeration(Enum value)
        {
            return new Enumeration(value.GetType()) { Name = value.ToString() };
        }
    }
}
