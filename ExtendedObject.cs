using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Knx
{
    [DataContract]
    public abstract class ExtendedObject : Object
    {
        // TODO: Wenn alles läuft, bitte mal testen, ob man das Feld in "Value" umbenennen kann. Es gab bereits Probleme, weil es öfters ein Feld namens _value gibt!
        [DataMember(Name = "Values")]
        public Dictionary<string, object> _values = new Dictionary<string, object>();

        /// <summary>
        /// Stores the value inside a dictionary by default. This method can be overridden.
        /// </summary>
        /// <param name="name">The name of the Property</param>
        /// <param name="value">The value of the Property</param>
        /// <returns>true, if the property was stored/changed, false if the property was not stored/changed</returns>
        protected virtual bool StorePropertyValue(string name, object value)
        {
            if (_values.ContainsKey(name))
            {
                if (_values[name] == null && value == null)
                    return false;

                if (_values[name] != null && _values[name].Equals(value))
                    return false;
                _values[name] = value;
            }
            else
            {
                _values.Add(name, value);
            }
            return true;
        }

        protected virtual bool LoadPropertyValue<T>(out T value, string name)
        {
            if (_values.ContainsKey(name))
            {
                value = (T)_values[name];
                return true;
            }
            value = default(T);
            return false;
        }

        protected T Get<T>(string name)
        {
            return Get(name, default(T));
        }

        protected T Get<T>(string name, T defaultValue)
        {
            T result;
            if (LoadPropertyValue<T>(out result, name))
                return result;

            return defaultValue;
        }

        protected T Get<T>(string name, Func<T> initialValue)
        {
            T result;
            if (LoadPropertyValue<T>(out result, name))
                return result;

            Set(name, initialValue());
            return Get<T>(name);
        }

        protected T Get<T>(Expression<Func<T>> expression)
        {
            return Get<T>(ObjectHelper.GetPropertyName(expression));
        }

        protected T Get<T>(Expression<Func<T>> expression, T defaultValue)
        {
            return Get(ObjectHelper.GetPropertyName(expression), defaultValue);
        }

        protected T Get<T>(Expression<Func<T>> expression, Func<T> initialValue)
        {
            return Get(ObjectHelper.GetPropertyName(expression), initialValue);
        }

        protected virtual bool Set<T>(string name, T value)
        {
            return StorePropertyValue(name, value);
        }

        protected bool Set<T>(Expression<Func<T>> expression, T value)
        {
            return Set(ObjectHelper.GetPropertyName(expression), value);
        }

        /// <summary>
        /// Clones the values from the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        public ExtendedObject CloneFrom(ExtendedObject source)
        {
            _values.Clear();

            foreach (var kvp in source._values)
                StorePropertyValue(kvp.Key, kvp.Value);

            return this;
        }
    }
}