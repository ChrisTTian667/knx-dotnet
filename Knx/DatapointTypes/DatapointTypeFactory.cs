using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes
{
    public static class DatapointTypeFactory
    {
        private static readonly object CreationLock = new object();

        #region Public Methods

        public static T Create<T>() where T : DatapointType
        {
            return (T)Create(typeof(T));
        }

        public static DatapointType Create(string id)
        {
            return Create(GetTypeById(id));
        }

        public static DatapointType Create(Type datapointTypeType)
        {
            lock (CreationLock)
            {
                var dataLengthAttribute = datapointTypeType.GetFirstCustomAttribute<DataLengthAttribute>(true);
                var dataLength = dataLengthAttribute.MinimumRequiredBytes;
                if (dataLength < 0)
                    dataLength = 0;
                
                var defaultPayload = new byte[dataLength];

                if (!(Activator.CreateInstance(datapointTypeType, defaultPayload) is DatapointType instance))
                    throw new InvalidOperationException($"The type '{datapointTypeType}' is no {typeof(DatapointType)}");

                return instance;
            }
        }

        #endregion

        #region Methods

        private static IEnumerable<Type> GetDatapointTypes()
        {
            return typeof(DatapointType).GetTypeInfo().Assembly.DefinedTypes.Where(t => t.GetCustomAttributes(typeof(DatapointTypeAttribute), false).Any()).Where(t => !t.IsAbstract).Select(ti => ti.AsType());
        }

        private static Type GetTypeById(string id)
        {
            var type = GetDatapointTypes().FirstOrDefault(t => t.GetFirstCustomAttribute<DatapointTypeAttribute>(true).ToString() == id);
            if (type == null)
                throw new Exception($"Unable to find DatapointType with id: '{id}'");

            return type;
        }

        #endregion
    }
}