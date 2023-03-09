using Knx.DatapointTypes.Dpt4ByteFloatValue;
using NUnit.Framework;

namespace Knx.Tests;

public class DatapointTypes14XXXTests
{
    [Test]
    public void DptValue4CountTest()
    {
        var dpt1 = new DptAcceleration(float.MaxValue);
        var dpt2 = new DptAcceleration(dpt1.Payload);
        var dpt3 = new DptAcceleration(float.MinValue);
        var dpt4 = new DptAcceleration(dpt3.Payload);
        var dpt5 = new DptAcceleration(13.37f);
        var dpt6 = new DptAcceleration(dpt5.Payload);

        Assert.AreEqual(float.MaxValue, dpt2.Value);
        Assert.AreEqual(float.MinValue, dpt4.Value);
        Assert.AreEqual(13.37f, dpt6.Value);
    }

    [Test]
    public void DptValue4CountTest2()
    {
        var dpt1 = new DptAcceleration(13.37f);
        var dpt2 = new DptAcceleration(dpt1.Payload);

        Assert.AreEqual(13.37f, dpt2.Value);
    }
}