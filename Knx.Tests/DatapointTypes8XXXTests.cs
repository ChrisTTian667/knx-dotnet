using System;
using Knx.DatapointTypes.Dpt2ByteSignedValue;
using NUnit.Framework;

namespace Knx.Tests;

public class DatapointTypes8XXXTests
{
    [Test]
    public void DptDeltaTimePeriodMillisecondsTest()
    {
        var dpt1 = new DptDeltaTimePeriodMilliseconds(TimeSpan.FromSeconds(-30));
        var dpt2 = new DptDeltaTimePeriodMilliseconds(dpt1.Payload);

        Assert.AreEqual(dpt2.Value, TimeSpan.FromSeconds(-30));
    }

    [Test]
    public void DptDeltaTimePeriod10MillisecondsTest()
    {
        var dpt1 = new DptDeltaTimePeriod10Milliseconds(TimeSpan.FromSeconds(-327.68));
        var dpt2 = new DptDeltaTimePeriod10Milliseconds(dpt1.Payload);

        Assert.AreEqual(dpt2.Value.TotalSeconds, TimeSpan.FromSeconds(-327.68).TotalSeconds);
    }

    [Test]
    public void DptDeltaTimePeriod100MillisecondsTest()
    {
        var dpt1 = new DptDeltaTimePeriod100Milliseconds(TimeSpan.FromSeconds(-1000));
        var dpt2 = new DptDeltaTimePeriod100Milliseconds(dpt1.Payload);

        Assert.AreEqual(dpt2.Value.TotalSeconds, TimeSpan.FromSeconds(-1000).TotalSeconds);
    }

    [Test]
    public void DptDeltaTimePeriodSecondsTest()
    {
        var dpt1 = new DptDeltaTimePeriodSeconds(TimeSpan.FromSeconds(-32768));
        var dpt2 = new DptDeltaTimePeriodSeconds(dpt1.Payload);

        Assert.AreEqual(dpt2.Value.TotalSeconds, TimeSpan.FromSeconds(-32768).TotalSeconds);
    }

    [Test]
    public void DptDeltaTimePeriodMinutesTest()
    {
        var dpt1 = new DptDeltaTimePeriodMinutes(TimeSpan.FromMinutes(-32768));
        var dpt2 = new DptDeltaTimePeriodMinutes(dpt1.Payload);

        Assert.AreEqual(dpt2.Value.TotalSeconds, TimeSpan.FromMinutes(-32768).TotalSeconds);
    }

    [Test]
    public void DptDeltaTimePeriodHoursTest()
    {
        var dpt1 = new DptDeltaTimePeriodHours(TimeSpan.FromHours(-32768));
        var dpt2 = new DptDeltaTimePeriodHours(dpt1.Payload);

        Assert.AreEqual(dpt2.Value.TotalSeconds, TimeSpan.FromHours(-32768).TotalSeconds);
    }
}