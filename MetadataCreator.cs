using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Knx.Common;
using Knx.DatapointTypes;
using Knx.Resources;

namespace Knx
{

    internal static class MetadataCreator
    {
        private static readonly object FactoryLock = new object();

        private static Collection<IPropertyInfoFactory> _propertyFactories;

        private static Collection<IPropertyInfoFactory> PropertyInfoFactories
        {
            get
            {
                lock (FactoryLock)
                {
                    if (_propertyFactories != null) 
                        return _propertyFactories;
                    
                    _propertyFactories = new Collection<IPropertyInfoFactory>();

                    var propertyInfoFactories = typeof(IPropertyInfoFactory).GetTypeInfo().Assembly.GetTypes().Where(
                        t => typeof(IPropertyInfoFactory).IsAssignableFrom(t.GetTypeInfo().AsType()) && !t.IsAbstract).ToArray();

                    foreach (var factory in propertyInfoFactories)
                        _propertyFactories.Add((IPropertyInfoFactory)Activator.CreateInstance(factory.GetTypeInfo().AsType()));
                }
                return _propertyFactories;
            }
        }

        internal static IEnumerable<Type> GetAllDatapointTypeTypes()
        {
            return typeof(DatapointType).GetTypeInfo().Assembly.GetTypes()
                .Where(dpt => dpt.GetCustomAttributes(typeof(DatapointTypeAttribute), false).Any() && !dpt.IsAbstract).Select(info => info.GetTypeInfo().AsType());
        }
        
        private static IEnumerable<PropertyInfo> GetDatatypeProperties(Type datapointTypeType)
        {
            if (!typeof(DatapointType).IsAssignableFrom(datapointTypeType))
            {
                throw new Exception("type must be assignable from DatapointType");
            }

            var propertyInfos = datapointTypeType.GetRuntimeProperties().Where(
                pi => pi.GetCustomAttributes(typeof(DatapointPropertyAttribute), true).Any()).ToArray();

            return propertyInfos;
        }

        private static bool ReadDatapointTypeLength(Type datapointType, out int length)
        {
            length = -1;

            var lengthAttribute =  datapointType.GetCustomAttributes(typeof(DataLengthAttribute), true).FirstOrDefault() as DataLengthAttribute;
            if (lengthAttribute == null) 
                return false;
            
            length = lengthAttribute.Length;
            return true;
        }

        private static bool ReadDatapointTypeId(Type datapointType, out string id, out string description)
        {
            id = string.Empty;
            description = string.Empty;

            var datapointTypeAttribute = datapointType.GetCustomAttributes(typeof(DatapointTypeAttribute), true).FirstOrDefault() as DatapointTypeAttribute;
            if (datapointTypeAttribute == null) 
                return false;
            
            id = datapointTypeAttribute.ToString();
            description = datapointTypeAttribute.Description;

            if (string.IsNullOrWhiteSpace(description))
            {
                description = Strings.ResourceManager.GetString(datapointType.Name);
                if (datapointType.Name.ToLower().StartsWith("dpt"))
                    description = datapointType.Name.Remove(0, 3);
            }

            return true;
        }
    }
}