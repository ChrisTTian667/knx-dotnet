using System;
using Knx.DatapointTypes;
using NUnit.Framework;

namespace Knx.Tests;

public class DatapointTypes19XXXTests
{
    [Test]
    public void DptDateTimeTest()
    {
        var now = DateTime.Now;

        var dpt1 = new DptDateTime(now);
        var dpt2 = new DptDateTime(dpt1.Payload);

        Assert.AreEqual(now.Year, dpt2.Year);
        Assert.AreEqual(now.Month, dpt2.Month);
        Assert.AreEqual(now.Day, dpt2.Day);

        Assert.AreEqual(now.Hour, dpt2.Hour);
        Assert.AreEqual(now.Minute, dpt2.Minute);
        Assert.AreEqual(now.Second, dpt2.Second);

        Assert.AreEqual(now.IsDaylightSavingTime(), dpt2.IsSummerTime);

        Assert.IsFalse(dpt2.IsFault);

        Assert.IsFalse(dpt2.IsYearValid);
        Assert.IsFalse(dpt2.IsWorkingDayValid);
        Assert.IsFalse(dpt2.IsDayOfWeekValid);
        Assert.IsFalse(dpt2.AreHoursMinutesSecondsValid);
        Assert.IsFalse(dpt2.AreMonthAndDayOfMonthValid);
    }
}