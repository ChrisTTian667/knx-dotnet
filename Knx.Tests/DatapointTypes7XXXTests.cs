using System;
using Knx.DatapointTypes.Dpt2ByteUnsignedValue;
using NUnit.Framework;

namespace Knx.Tests;

public class DatapointTypes7XXXTests
{
    [Test]
    public void Dpt2ByteUCountTest()
    {
        ushort[] intarray = { 17000, 0, 65535 };

        foreach (var value in intarray)
        {
            var dpt1 = new DptValue2UCount(value);
            var dpt2 = new DptValue2UCount(dpt1.Payload);
            Assert.AreEqual(dpt2.Value, value);
        }
    }

    [Test]
    public void DptPropDataTypeTest()
    {
        ushort[] intarray = { 17000, 0, 65535 };

        foreach (var value in intarray)
        {
            var dpt1 = new DptPropDataType(value);
            var dpt2 = new DptPropDataType(dpt1.Payload);
            Assert.AreEqual(dpt2.Value, value);
        }
    }

    [Test]
    public void DptTimePeriodMillisecondsTest()
    {
        var dpt1 = new DptTimePeriodMilliseconds(TimeSpan.FromSeconds(60));
        var dpt2 = new DptTimePeriodMilliseconds(dpt1.Payload);

        Assert.AreEqual(dpt2.Value, TimeSpan.FromSeconds(60));
    }

    [Test]
    public void DptTimePeriod10MillisecondsTest()
    {
        var dpt1 = new DptTimePeriod10Milliseconds(TimeSpan.FromSeconds(655.35));
        var dpt2 = new DptTimePeriod10Milliseconds(dpt1.Payload);

        Assert.AreEqual(dpt2.Value.TotalSeconds, TimeSpan.FromSeconds(655.35).TotalSeconds);
    }

    [Test]
    public void DptTimePeriod100MillisecondsTest()
    {
        var dpt1 = new DptTimePeriod100Milliseconds(TimeSpan.FromSeconds(6553.5));
        var dpt2 = new DptTimePeriod100Milliseconds(dpt1.Payload);

        Assert.AreEqual(dpt2.Value.TotalSeconds, TimeSpan.FromSeconds(6553.5).TotalSeconds);
    }

    [Test]
    public void DptTimePeriodSecondsTest()
    {
        var dpt1 = new DptTimePeriodSeconds(TimeSpan.FromSeconds(65535));
        var dpt2 = new DptTimePeriodSeconds(dpt1.Payload);

        Assert.AreEqual(dpt2.Value.TotalSeconds, TimeSpan.FromSeconds(65535).TotalSeconds);
    }

    [Test]
    public void DptTimePeriodMinutesTest()
    {
        var dpt1 = new DptTimePeriodMinutes(TimeSpan.FromMinutes(65535));
        var dpt2 = new DptTimePeriodMinutes(dpt1.Payload);

        Assert.AreEqual(dpt2.Value.TotalHours, TimeSpan.FromMinutes(65535).TotalHours);
    }

    [Test]
    public void DptTimePeriodHoursTest()
    {
        var dpt1 = new DptTimePeriodHours(TimeSpan.FromHours(65535));
        var dpt2 = new DptTimePeriodHours(dpt1.Payload);

        Assert.AreEqual(dpt2.Value.TotalHours, TimeSpan.FromHours(65535).TotalHours);
    }
}