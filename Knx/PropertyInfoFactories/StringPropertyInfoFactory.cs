using System;
using System.Reflection;

namespace Knx.PropertyInfoFactories
{
    internal class StringPropertyInfoFactory : PropertyInfoFactory
    {
        private bool IsSupportedType(Type type)
        {
            return typeof(string).IsAssignableFrom(type)
                   || typeof(char).IsAssignableFrom(type);
        }

        protected override IDatatypePropertyInfo CreatePropertyInfo(string propertyName, PropertyInfo propertyInfo, Type datapointTypeType)
        {
            if (!IsSupportedType(propertyInfo.PropertyType))
            {
                return null;
            }

            var propertyInfoType = typeof(DatatypePropertyInfo<>).MakeGenericType(typeof(String));
            var unit = PropertyFactoryHelper.GetDatapointTypePropertyUnit(datapointTypeType, propertyInfo);

            return (IDatatypePropertyInfo)Activator.CreateInstance(propertyInfoType, propertyName, unit);
        }
    }
}