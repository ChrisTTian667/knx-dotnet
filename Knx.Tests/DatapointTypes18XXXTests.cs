using Knx.DatapointTypes;
using NUnit.Framework;

namespace Knx.Tests;

public class DatapointTypes18XXXTests
{
    [Test]
    public void DptSceneControlTest()
    {
        var dpt1 = new DptSceneControl(DptSceneControl.SceneControl.Activate, 17);
        var dpt2 = new DptSceneControl(dpt1.Payload);

        Assert.AreEqual(17, dpt2.Scene);
        Assert.AreEqual(DptSceneControl.SceneControl.Activate, dpt2.Control);
    }

    [Test]
    public void DptSceneControlTest2()
    {
        var dpt1 = new DptSceneControl(DptSceneControl.SceneControl.Learn, 63);
        var dpt2 = new DptSceneControl(dpt1.Payload);

        Assert.AreEqual(63, dpt2.Scene);
        Assert.AreEqual(DptSceneControl.SceneControl.Learn, dpt2.Control);
    }
}