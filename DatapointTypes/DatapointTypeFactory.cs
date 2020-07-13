using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Knx.Common;

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
                {
                    dataLength = 0;
                }
                var defaultPayload = new byte[dataLength];

                var instance = Activator.CreateInstance(datapointTypeType, new object[] { defaultPayload }) as DatapointType;

                if (instance == null)
                {
                    throw new InvalidOperationException(string.Format("The type '{0}' is no {1}", datapointTypeType, typeof(DatapointType)));
                }

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
            {
                throw new Exception(string.Format("Unable to find DatapointType with id: '{0}'", id));
            }

            return type;
        }

        #endregion
    }
}