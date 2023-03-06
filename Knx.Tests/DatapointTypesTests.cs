using System;
using System.Collections.Generic;
using System.Linq;
using Knx.Common;
using Knx.Common.Attribute;
using Knx.DatapointTypes;
using Knx.DatapointTypes.Dpt1Bit;
using Knx.DatapointTypes.Dpt2Bit;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Knx.Tests;

/// <summary>
///     General Datapoint types test
/// </summary>
public class DatapointTypesTests
{
    [Test]
    public void PayloadConversion()
    {
        var byteArray = new byte[] { 0, 1, 2, 255, 255 };
        var readableString = byteArray.ToReadableString();
        var result = ByteArrayExtensions.FromReadableString(readableString);
        Assert.AreEqual(byteArray, result);
    }

    [Test]
    public void CreateSimpleDatapointType()
    {
        var boolDpt = DatapointTypeFactory.Create("1.001", new byte[] { 1 });
        Assert.IsNotNull(boolDpt);
    }

    [Test]
    public void SerializeSimpleBooleanDatapointType()
    {
        var dpt = DatapointTypeFactory.Create("1.001", new byte[] { 1 });
        var serializedObject = JsonConvert.SerializeObject(dpt);
        var deserializeObject = JsonConvert.DeserializeObject<DptBoolean>(serializedObject);

        Assert.IsNotNull(dpt);
        Assert.AreEqual(dpt.Payload, deserializeObject.Payload);
    }

    [Test]
    public void SerializeDptAlarmControl()
    {
        var dpt = DatapointTypeFactory.Create(typeof(DptAlarmControl), new byte[] { 1 });
        var serializedObject = JsonConvert.SerializeObject(dpt);
        var deserializeObject = JsonConvert.DeserializeObject<DptAlarmControl>(serializedObject);

        Assert.IsNotNull(dpt);
        Assert.AreEqual(dpt.Payload, deserializeObject.Payload);
    }

    [Test]
    public void DeserializeSpecificToBaseType()
    {
        var dpt = DatapointTypeFactory.Create(typeof(DptAlarmControl), new byte[] { 1 });
        var serializedSpecific = JsonConvert.SerializeObject(dpt);

        var deserializeBase = JsonConvert.DeserializeObject<DatapointType>(serializedSpecific);
        var serializedBase = JsonConvert.SerializeObject(deserializeBase);

        var deserializeSpecific = JsonConvert.DeserializeObject<DptAlarmControl>(serializedBase);

        Assert.IsNotNull(dpt);
        Assert.AreEqual(dpt.Payload, deserializeSpecific.Payload);
    }

    [Test]
    public void EachDatapointType_Serialize_NoException()
    {
        var count = 0;
        foreach (var type in GetDatapointTypes()
                     .OrderBy(
                         t => ((DatapointTypeAttribute)t.GetCustomAttributes(typeof(DatapointTypeAttribute), true)
                             .First())?.ToString()))
            try
            {
                var dpt = DatapointTypeFactory.Create(type);
                var serializeDpt = JsonConvert.SerializeObject(dpt);
                var deserializeDpt = (DatapointType)JsonConvert.DeserializeObject(serializeDpt, type);

                Assert.IsNotNull(deserializeDpt);
                Assert.IsNotEmpty(deserializeDpt.DatapointTypeId);

                count++;
            }
            catch (MissingMethodException ex)
            {
                Assert.Fail($"Type {type}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Type {type}: {ex.Message}");
            }

        Assert.AreEqual(GetCountOfDatapointTypes(), count);
    }

    [Test]
    public void EachDatapointTypeHasDataLengthAttribute()
    {
        var datapointTypesCount = GetCountOfDatapointTypes();
        var allDatapointTypesWithDataLengthAttributeCount =
            typeof(DatapointType).Assembly.GetTypes()
                .Where(
                    t => t.GetCustomAttributes(typeof(DatapointTypeAttribute), false).Any())
                .Where(t => t.GetCustomAttributes(typeof(DataLengthAttribute), true).Length == 1)
                .Count(t => !t.IsAbstract);

        Assert.AreEqual(datapointTypesCount, allDatapointTypesWithDataLengthAttributeCount);
    }

    [Test]
    public void CreateEachDatapointType_NoException()
    {
        var count = 0;
        foreach (var type in GetDatapointTypes()
                     .OrderBy(t => t.GetCustomAttributes(typeof(DatapointTypeAttribute), true).First()?.ToString()))
            try
            {
                var dpt = DatapointTypeFactory.Create(type);
                Assert.IsNotNull(dpt);

                count++;
            }
            catch (MissingMethodException ex)
            {
                Assert.Fail($"Type {type}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Type {type}: {ex.Message}");
            }

        Assert.AreEqual(GetCountOfDatapointTypes(), count);
    }

    private static int GetCountOfDatapointTypes()
    {
        return GetDatapointTypes().Count();
    }

    private static IEnumerable<Type> GetDatapointTypes()
    {
        return typeof(DatapointType).Assembly.GetTypes()
            .Where(t => t.GetCustomAttributes(typeof(DatapointTypeAttribute), false).Any())
            .Where(t => !t.IsAbstract);
    }
}
