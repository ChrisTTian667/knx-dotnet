using System;
using Knx.PropertyInfoFactories;

namespace Knx
{
    /// <summary>
    /// Marks a class as a Datatype property.
    /// </summary>
    public interface IDatatypeProperty
    {
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        IDatatypePropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        Type PropertyType { get; }

        /// <summary>
        /// Gets or sets the value (or placeholder).
        /// </summary>
        IValue Value { get; set; }

        /// <summary>
        /// Get the Default Value for this Property (this is never a Placeholder)
        /// </summary>
        IValue DefaultValue { get; }

        /// <summary>
        /// Get the DisplayText of the current Value, or if null, from the DefaultValue
        /// </summary>
        string ValueDisplayText { get; }
    }
    
    /// <summary>
    /// Generic Property.
    /// </summary>
    /// <typeparam name="T">Datatype of the value.</typeparam>
    public sealed class DatatypeProperty<T> : BindableObject, IDatatypeProperty
    {
        #region Constructors and Destructors

        public DatatypeProperty(IDatatypePropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }

        #endregion

        #region Properties

        public IDatatypePropertyInfo PropertyInfo
        {
            get
            {
                return Get(() => PropertyInfo);
            }

            private set
            {
                Set(() => PropertyInfo, value);
            }
        }

        public string PropertyName => PropertyInfo.Name;

        public Type PropertyType => PropertyInfo.PropertyType;

        public IValue<T> Value
        {
            get
            {
                return Get(() => Value);
            }

            set
            {
                Set(() => Value, value);
                RaisePropertyChanged(() => this.ValueDisplayText);
            }
        }

        IDatatypePropertyInfo IDatatypeProperty.PropertyInfo => PropertyInfo;

        IValue IDatatypeProperty.Value
        {
            get
            {
                return Get(() => Value);
            }

            set
            {
                if (!(value is IValue<T>))
                {
                    throw new Exception(string.Format("Value must be of type IValue<{0}>.", typeof(T)));
                }

                Value = (IValue<T>)value;
            }
        }        

        #endregion

        public string ValueDisplayText
        {
            get
            {
                if (Value != null)
                    return PropertyInfo.GetDisplayValue(Value.Current);
                else
                    return PropertyInfo.GetDisplayValue(DefaultValue.Current);
            }
        }

        public IValue DefaultValue
        {
            get
            {
                return (IValue)Activator.CreateInstance(typeof(Value<T>), default(T));
            }
        }
    }
}