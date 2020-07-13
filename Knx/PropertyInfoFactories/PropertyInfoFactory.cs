using System;
using System.Linq;
using System.Reflection;
using Knx.Common;

namespace Knx.PropertyInfoFactories
{
    public abstract class PropertyInfoFactory : IPropertyInfoFactory
    {
        public IDatatypePropertyInfo Create(PropertyInfo propertyInfo, Type datapointTypeType)
        {
            var propertyAttribute =
                propertyInfo.GetCustomAttributes(typeof(DatapointPropertyAttribute), true).FirstOrDefault() as
                DatapointPropertyAttribute;

            if (propertyAttribute == null)
            {
                throw new Exception("Property: " + propertyInfo.Name + " has not DatapointProperty attribute.");
            }

            var propertyName = string.IsNullOrWhiteSpace(propertyAttribute.PropertyName) ? propertyInfo.Name : propertyAttribute.PropertyName;

            return CreatePropertyInfo(propertyName, propertyInfo, datapointTypeType);
        }

        protected abstract IDatatypePropertyInfo CreatePropertyInfo(string propertyName, PropertyInfo propertyInfo, Type datapointTypeType);
    }

    public abstract class PropertyInfoFactory<T> : PropertyInfoFactory where T : IDatatypePropertyInfo
    {
        protected override IDatatypePropertyInfo CreatePropertyInfo(string propertyName, PropertyInfo propertyInfo, Type datapointTypeType)
        {
            return Create(propertyName, propertyInfo, datapointTypeType);
        }

        protected abstract T Create(string propertyName, PropertyInfo propertyInfo, Type datapointTypeType);
    }
}