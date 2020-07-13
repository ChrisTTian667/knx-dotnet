using System;
using System.Reflection;
using Knx.DatatypePropertyInfos;

namespace Knx.PropertyInfoFactories
{
    internal class EnumerationPropertyInfoFactory : PropertyInfoFactory<EnumerationPropertyInfo>
    {
        protected override EnumerationPropertyInfo Create(string propertyName, PropertyInfo propertyInfo, Type datapointTypeType)
        {
            return !propertyInfo.PropertyType.GetTypeInfo().IsEnum 
                ? default 
                : new EnumerationPropertyInfo(propertyName, PropertyFactoryHelper.GetDatapointTypePropertyUnit(datapointTypeType, propertyInfo), new Enumeration(propertyInfo.PropertyType).Names);
        }
    }
}