using System;
using System.Reflection;
using Knx.DatatypePropertyInfos;

namespace Knx.PropertyInfoFactories
{
    internal class DayOfWeekPropertyInfoFactory : PropertyInfoFactory<DayOfWeekPropertyInfo>
    {
        private bool IsSupportedType(Type type)
        {
            return typeof(DayOfWeek).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo())
                   || typeof(DayOfWeek?).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());
        }

        protected override DayOfWeekPropertyInfo Create(string propertyName, PropertyInfo propertyInfo, Type datapointTypeType)
        {
            if (!IsSupportedType(propertyInfo.PropertyType))
            {
                return null;
            }

            return new DayOfWeekPropertyInfo(propertyName, PropertyFactoryHelper.GetDatapointTypePropertyUnit(datapointTypeType, propertyInfo), propertyInfo.PropertyType, new Enumeration(propertyInfo.PropertyType).Names)
                {
                    IsNullable = propertyInfo.PropertyType.Name.StartsWith("Nullable")
                };
        }
    }
}