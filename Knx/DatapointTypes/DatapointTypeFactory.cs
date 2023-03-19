using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes;

public static class DatapointTypeFactory
{
    public static T Create<T>(byte[]? payload = null)
        where T : DatapointType =>
            (T)Create(typeof(T), payload);

    public static DatapointType Create(string id, byte[]? payload = null)
    {
        var datapointType = DatapointTypesCache.Value.WithId(id);
        if (datapointType is null)
            throw new NotSupportedException($"DatapointType '{id}' is not supported.");

        return Create(datapointType, payload);
    }

    public static void GetInfo(string id)
    {
        var dpt = Create(id);





    }

    private static DatapointType Create(Type datapointTypeType, byte[]? value = null)
    {
        var dataLengthAttribute = datapointTypeType
            .GetCustomAttributes<DataLengthAttribute>(true)
            .FirstOrDefault();

        var defaultPayload = new byte[Math.Max(dataLengthAttribute?.MinimumRequiredBytes ?? 0, 0)];

        if (Activator.CreateInstance(datapointTypeType, defaultPayload) is not DatapointType instance)
            throw new InvalidOperationException($"The '{datapointTypeType}' is not a {typeof(DatapointType)}");

        if (value != null)
            instance.Payload = value;

        return instance;
    }

    private static Type? WithId(this IEnumerable<Type> types, string id) =>
        types.FirstOrDefault(t => t.GetCustomAttributes(typeof(DatapointTypeAttribute), true)
            .FirstOrDefault()?.ToString() == id);

    private static readonly Lazy<IEnumerable<Type>> DatapointTypesCache = new(() =>
        typeof(DatapointType).GetTypeInfo()
            .Assembly.DefinedTypes
            .Where(t => t.GetCustomAttributes(typeof(DatapointTypeAttribute), false).Any())
            .Where(t => !t.IsAbstract)
            .Select(ti => ti.AsType())
            .ToList()
    );
}
