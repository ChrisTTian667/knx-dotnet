using Knx.DatapointTypes.Dpt2ByteFloat;
using NUnit.Framework;

namespace Knx.Tests;

public class DatapointTypes9XXXTests
{
    [Test]
    public void TemperatureTest()
    {
        const double tempValue = 19.2;

        var datapointTemperature = new DptTemperature(tempValue);
        var bytes = datapointTemperature.Payload;

        var datapointTemperature2 = new DptTemperature(bytes);

        Assert.AreEqual(tempValue, datapointTemperature2.Value);
    }

    //[Test]
    //public void RainAmountTest()
    //{
    //    var amount = 670500;

    //    var rainAmount = new DptRainAmount(amount);
    //    var bytes = rainAmount.Payload;

    //    var rainAmount2 = new DptRainAmount(bytes);

    //    Assert.AreEqual(amount, rainAmount2.Value);
    //}
}