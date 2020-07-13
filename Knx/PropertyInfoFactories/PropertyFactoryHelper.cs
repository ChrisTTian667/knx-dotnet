using System;
using System.Linq;
using System.Reflection;
using Knx.Common;
using Knx.Resources;

namespace Knx.PropertyInfoFactories
{
    internal static class PropertyFactoryHelper
    {
        private static string GetDatapointTypeUnit(Type datapointTypeType)
        {
            var dptAttribute = datapointTypeType.GetCustomAttributes(typeof(DatapointTypeAttribute), true).FirstOrDefault() as DatapointTypeAttribute;
            return dptAttribute != null ? Strings.ResourceManager.GetString(dptAttribute.Unit.ToString()) : string.Empty;
        }

        internal static string GetDatapointTypePropertyUnit(Type datapointTypeType, PropertyInfo property)
        {
            var unit = string.Empty;

            var propertyAttribute = property.GetCustomAttributes(typeof(DatapointPropertyAttribute), true).FirstOrDefault() as DatapointPropertyAttribute;
            if (propertyAttribute != null)
                return Resources.Strings.ResourceManager.GetString(propertyAttribute.Unit.ToString());

            // Fallback (get unit of DatapointType)
            if (string.IsNullOrWhiteSpace(unit))
                unit = GetDatapointTypeUnit(datapointTypeType);

            return unit;
        }
    }
}