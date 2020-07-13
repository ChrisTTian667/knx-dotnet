using System;
using Knx.PropertyInfoFactories;

namespace Knx
{
    public class DatatypePropertyInfo<T> : BindableObject, IDatatypePropertyInfo
    {
        #region Constructors and Destructors

        public DatatypePropertyInfo(string name, string unit = "")
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentOutOfRangeException(nameof(name), "Name must be a valid string value.");
           
            Name = name;
            Unit = unit;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the real type of the property.
        /// </summary>
        /// <value>
        /// The type of the property.
        /// </value>
        public Type PropertyType { get; protected set; } = typeof(T);

        public virtual bool IsNullable { get; set; }

        public string Unit { get; private set; }

        public virtual string GetDisplayValue(object value)
        {
            return value != null ? $"{value} {Unit}" : string.Empty;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}