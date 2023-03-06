using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes;

public static class DatapointTypeFactory
{
    private static readonly object CreationLock = new();

    #region Public Methods

    public static T Create<T>() where T : DatapointType
    {
        return (T)Create(typeof(T));
    }

    public static DatapointType Create(string id, byte[] payload = null)
    {
        return Create(GetTypeById(id), payload);
    }

    public static DatapointType Create(Type datapointTypeType, byte[] value = null)
    {
        lock (CreationLock)
        {
            var dataLengthAttribute =
                (DataLengthAttribute)datapointTypeType.GetCustomAttributes(typeof(DataLengthAttribute), true).First();
            var dataLength = dataLengthAttribute.MinimumRequiredBytes;
            if (dataLength < 0)
                dataLength = 0;

            var defaultPayload = new byte[dataLength];

            if (!(Activator.CreateInstance(datapointTypeType, defaultPayload) is DatapointType instance))
                throw new InvalidOperationException($"The type '{datapointTypeType}' is no {typeof(DatapointType)}");

            if (value != null)
                instance.Payload = value;

            return instance;
        }
    }

    public static IEnumerable<string> GetSupportedDatapointTypeIds()
    {
        return GetDatapointTypes()
            .Select(
                dptType =>
                    ((DatapointTypeAttribute)dptType.GetCustomAttributes(typeof(DatapointTypeAttribute), false).First())
                    .ToString());
    }

    #endregion

    #region Methods

    private static IEnumerable<Type> GetDatapointTypes()
    {
        return typeof(DatapointType).GetTypeInfo()
            .Assembly.DefinedTypes.Where(t => t.GetCustomAttributes(typeof(DatapointTypeAttribute), false).Any())
            .Where(t => !t.IsAbstract)
            .Select(ti => ti.AsType());
    }

    private static Type GetTypeById(string id)
    {
        var type = GetDatapointTypes()
            .FirstOrDefault(t => t.GetCustomAttributes(typeof(DatapointTypeAttribute), true).First().ToString() == id);

        if (type == null)
            throw new Exception($"Unable to find DatapointType with id: '{id}'");

        return type;
    }

    #endregion
}