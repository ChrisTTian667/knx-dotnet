using System;
using System.Linq;
using System.Reflection;
using Knx.Common;
using Knx.Common.Exceptions;
using Knx.DatatypePropertyInfos;

namespace Knx.PropertyInfoFactories
{
    internal class NumericPropertyInfoFactory : PropertyInfoFactory
    {
        private bool IsSupportedType(Type type)
        {
            return typeof(double).IsAssignableFrom(type)
                   || typeof(decimal).IsAssignableFrom(type)
                   || typeof(float).IsAssignableFrom(type)
                   || typeof(long).IsAssignableFrom(type)
                   || typeof(int).IsAssignableFrom(type)
                   || typeof(byte).IsAssignableFrom(type)
                   || typeof(sbyte).IsAssignableFrom(type)
                   || typeof(uint).IsAssignableFrom(type)
                   || typeof(UInt16).IsAssignableFrom(type)
                   || typeof(Int16).IsAssignableFrom(type)
                   || typeof(SByte).IsAssignableFrom(type);
        }

        protected override IDatatypePropertyInfo CreatePropertyInfo(string propertyName, PropertyInfo propertyInfo, Type datapointTypeType)
        {
            if (!IsSupportedType(propertyInfo.PropertyType))
            {
                return null;
            }

            var rangeAttribute = propertyInfo.GetCustomAttributes(typeof(RangeAttribute), true).FirstOrDefault() as RangeAttribute;

            var minValue = Convert.ChangeType((rangeAttribute != null ? rangeAttribute.Minimum : propertyInfo.PropertyType.GetField("MinValue").GetValue(null)), propertyInfo.PropertyType, null);
            var maxValue = Convert.ChangeType((rangeAttribute != null ? rangeAttribute.Maximum : propertyInfo.PropertyType.GetField("MaxValue").GetValue(null)), propertyInfo.PropertyType, null);
            var unit = PropertyFactoryHelper.GetDatapointTypePropertyUnit(datapointTypeType, propertyInfo);

            if (minValue == null || maxValue == null)
            {
                throw new KnxException(string.Format("Unable to create Metadata for type '{0}'. => Unable to retrieve MinValue & MaxValue.", propertyInfo.PropertyType));
            }

            return (IDatatypePropertyInfo)Activator.CreateInstance(typeof(NumericPropertyInfo), propertyName, unit, propertyInfo.PropertyType, minValue.ToString(), maxValue.ToString());

            //var propertyInfoType = typeof (PropertyInfoWithRange<>).MakeGenericType(propertyInfo.PropertyType);
            //return (IPropertyInfo)Activator.CreateInstance(propertyInfoType, propertyName, unit, minValue, maxValue);
        }
    }
}