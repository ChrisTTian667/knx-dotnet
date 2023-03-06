using Knx.DatapointTypes.Dpt8ByteSignedValue;
using NUnit.Framework;

namespace Knx.Tests;

public class DatapointTypes29XXXTests
{
    [Test]
    public void DptActiveEnergy64Test()
    {
        var dpt1 = new DptActiveEnergy64(long.MaxValue);
        var dpt2 = new DptActiveEnergy64(dpt1.Payload);

        var dpt3 = new DptActiveEnergy64(long.MinValue);
        var dpt4 = new DptActiveEnergy64(dpt3.Payload);

        Assert.AreEqual(dpt2.Value, long.MaxValue);
        Assert.AreEqual(dpt4.Value, long.MinValue);
    }

    [Test]
    public void DptApparantEnergy64Test()
    {
        var dpt1 = new DptApparantEnergy64(long.MaxValue);
        var dpt2 = new DptApparantEnergy64(dpt1.Payload);

        var dpt3 = new DptApparantEnergy64(long.MinValue);
        var dpt4 = new DptApparantEnergy64(dpt3.Payload);

        Assert.AreEqual(dpt2.Value, long.MaxValue);
        Assert.AreEqual(dpt4.Value, long.MinValue);
    }

    [Test]
    public void DptReactiveEnergy64Test()
    {
        var dpt1 = new DptReactiveEnergy64(long.MaxValue);
        var dpt2 = new DptReactiveEnergy64(dpt1.Payload);

        var dpt3 = new DptReactiveEnergy64(long.MinValue);
        var dpt4 = new DptReactiveEnergy64(dpt3.Payload);

        Assert.AreEqual(dpt2.Value, long.MaxValue);
        Assert.AreEqual(dpt4.Value, long.MinValue);
    }
}