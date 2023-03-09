using Knx.DatapointTypes.Dpt4ByteCombinedInfo;
using NUnit.Framework;

namespace Knx.Tests;

public class DatapointTypes27XXXTests
{
    [Test]
    public void Dpt4ByteCombinedInfoOnOffTest()
    {
        var dpt1 = new Dpt4ByteCombinedInfoOnOff(
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true);
        var dpt2 = new Dpt4ByteCombinedInfoOnOff(dpt1.Payload);

        Assert.AreEqual(byte.MaxValue, dpt2.Payload[0]);
        Assert.AreEqual(byte.MaxValue, dpt2.Payload[1]);
        Assert.AreEqual(byte.MaxValue, dpt2.Payload[2]);
        Assert.AreEqual(byte.MaxValue, dpt2.Payload[3]);
    }
}