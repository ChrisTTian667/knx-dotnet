using System;
using System.Linq;
using System.Reflection;
using Knx.Common;
using Knx.DatatypePropertyInfos;
using Knx.Resources;

namespace Knx.PropertyInfoFactories
{
    internal class BooleanPropertyInfoFactory : PropertyInfoFactory<BoolPropertyInfo>
    {
        protected override BoolPropertyInfo Create(string propertyName, PropertyInfo propertyInfo, Type datapointTypeType)
        {
            if (!typeof(bool).GetTypeInfo().IsAssignableFrom(propertyInfo.PropertyType.GetTypeInfo()))
            {
                return null;
            }

            var trueValue = Resources.Strings.Yes;
            var falseValue = Resources.Strings.No;

            var encodingAttribute = propertyInfo.GetCustomAttributes(typeof(BooleanEncodingAttribute), true).FirstOrDefault() as BooleanEncodingAttribute;
            if (encodingAttribute != null)
            {
                trueValue = Strings.ResourceManager.GetString(encodingAttribute.TrueEncoding.ToString());
                falseValue = Strings.ResourceManager.GetString(encodingAttribute.FalseEncoding.ToString());
            }

            return new BoolPropertyInfo(propertyName, PropertyFactoryHelper.GetDatapointTypePropertyUnit(datapointTypeType, propertyInfo), falseValue, trueValue);
        }
    }
}