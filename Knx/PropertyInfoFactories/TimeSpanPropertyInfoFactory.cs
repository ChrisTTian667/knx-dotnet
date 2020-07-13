using System;
using System.Reflection;
using Knx.DatatypePropertyInfos;

namespace Knx.PropertyInfoFactories
{
    internal class TimeSpanPropertyInfoFactory : PropertyInfoFactory<TimePropertyInfo>
    {
        private bool IsSupportedType(Type type)
        {
            return typeof(TimeSpan).IsAssignableFrom(type)
                   || typeof(DateTime).IsAssignableFrom(type);
        }

        protected override TimePropertyInfo Create(string propertyName, PropertyInfo propertyInfo, Type datapointTypeType)
        {
            if (!IsSupportedType(propertyInfo.PropertyType))
            {
                return null;
            }

            return new TimePropertyInfo(propertyName, PropertyFactoryHelper.GetDatapointTypePropertyUnit(datapointTypeType, propertyInfo));
        }
    }
}