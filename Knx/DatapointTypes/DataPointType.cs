using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes;

[DataContract]
public class DatapointType
{
    private static readonly Lazy<IEnumerable<Type>> DatapointTypes = new(
        () =>
            typeof(DatapointType)
                .Assembly
                .DefinedTypes
                .Where(t => !t.IsAbstract)
                .Select(t => new
                {
                    Type = t,
                    Dpt = t.GetCustomAttributes<DatapointTypeAttribute>(false)
                        .FirstOrDefault()
                })
                .Where(t => t.Dpt != null)
                .OrderBy(td => td.Dpt!.MainNumber)
                    .ThenBy(td => td.Dpt!.SubNumber)
                .Select(td => td.Type)
                .ToList());

    private byte[] _payload = Array.Empty<byte>();

    protected DatapointType()
    {
        var datapointType = GetType();
        if (datapointType != typeof(DatapointType))
        {
            DatapointTypeId = GetType()
                .GetCustomAttributes<DatapointTypeAttribute>(true)
                .First()
                .ToString();
        }
    }

    protected DatapointType(byte[] payload, bool verifyExactPayloadLength = false) : this()
    {
        Payload = payload;

        if (!VerifyPayload(payload, verifyExactPayloadLength, out var requiredBytes))
        {
            if (requiredBytes < 0)
                throw new ArgumentException("Payload verification failed", nameof(Payload));

            throw new ArgumentOutOfRangeException(
                nameof(Payload),
                string.Format(
                    verifyExactPayloadLength
                        ? "Payload needs to have a length of {0} bytes."
                        : "Payload needs at least a length {0} bytes.",
                    requiredBytes));
        }
    }

    [DataMember(Name = "Id", IsRequired = true)]
    public string DatapointTypeId { get; set; } = null!;

    public byte[] Payload
    {
        get => _payload;
        protected internal set
        {
            if (value.Length == 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(Payload),
                    "Datapoint Type needs at least one byte of data");
            }

            _payload = value;
        }
    }

    [DataMember(Name = nameof(Payload))]
    private string ReadablePayload
    {
        get => _payload.ToReadableString();
        set => _payload = ByteArrayExtensions.FromReadableString(value);
    }

    /// <summary>
    ///     Returns a list of types of all supported Datapoint types.
    /// </summary>
    public static IEnumerable<Type> All => DatapointTypes.Value;

    public static T Create<T>(byte[]? payload = null)
        where T : DatapointType
    {
        return (T)Create(typeof(T), payload);
    }

    public static DatapointType Create(string id, byte[]? payload = null)
    {
        var datapointType = All
            .FirstOrDefault(
                t => t.GetCustomAttributes<DatapointTypeAttribute>(true)
                    .First()
                    .ToString() == id);

        if (datapointType is null)
            throw new NotSupportedException($"DatapointType '{id}' is not supported.");

        return Create(datapointType, payload);
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

    private bool VerifyPayload(IReadOnlyCollection<byte> payload, bool exactMatch, out int requiredBytes)
    {
        var dataLengthAttribute = GetType()
            .GetCustomAttributes(typeof(DataLengthAttribute), true)
            .Cast<DataLengthAttribute>()
            .FirstOrDefault();

        if (dataLengthAttribute == null)
        {
            requiredBytes = -1;

            return true;
        }

        var isOk = !exactMatch
            ? payload.Count >= dataLengthAttribute.MinimumRequiredBytes
            : payload.Count == dataLengthAttribute.MinimumRequiredBytes;
        requiredBytes = dataLengthAttribute.MinimumRequiredBytes;

        return isOk;
    }

    public static implicit operator byte[](DatapointType dpt)
    {
        return dpt.Payload;
    }
}
