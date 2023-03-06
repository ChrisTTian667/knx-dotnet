using Knx.DatapointTypes.DptVariableString;
using NUnit.Framework;

namespace Knx.Tests;

public class DatapointTypes24XXXTests
{
    [Test]
    public void DptVariableString8859_1Test()
    {
        var dpt1 = new DptVariableString_8859_1("KNX is OK");
        var dpt2 = new DptVariableString_8859_1(dpt1.Payload);
        var dpt3 = new DptVariableString_8859_1(string.Empty); // The string will be automatically terminated.

        Assert.AreEqual("KNX is OK", dpt2.Value);

        Assert.AreEqual(0x4B, dpt2.Payload[0]);
        Assert.AreEqual(0x4E, dpt2.Payload[1]);
        Assert.AreEqual(0x58, dpt2.Payload[2]);
        Assert.AreEqual(0x20, dpt2.Payload[3]);
        Assert.AreEqual(0x69, dpt2.Payload[4]);
        Assert.AreEqual(0x73, dpt2.Payload[5]);
        Assert.AreEqual(0x20, dpt2.Payload[6]);
        Assert.AreEqual(0x4F, dpt2.Payload[7]);
        Assert.AreEqual(0x4B, dpt2.Payload[8]);
        Assert.AreEqual(0x00, dpt2.Payload[9]);
        Assert.AreEqual(10, dpt2.Payload.Length);

        Assert.AreEqual(0, dpt3.Payload[0]);
    }
}