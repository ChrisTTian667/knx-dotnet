using Knx.Common;
using NUnit.Framework;

namespace Knx.Tests;

public class ByteExtensionTests
{
    [Test]
    public void SetBitToFalseTest()
    {
        const byte previous = 0xFF;

        Assert.AreEqual(254, previous.SetBit(7, false));
        Assert.AreEqual(253, previous.SetBit(6, false));
        Assert.AreEqual(251, previous.SetBit(5, false));
        Assert.AreEqual(247, previous.SetBit(4, false));
        Assert.AreEqual(239, previous.SetBit(3, false));
        Assert.AreEqual(223, previous.SetBit(2, false));
        Assert.AreEqual(191, previous.SetBit(1, false));
        Assert.AreEqual(127, previous.SetBit(0, false));
    }

    [Test]
    public void SetBitToTrueTest2()
    {
        const byte previous = 0x00;

        Assert.AreEqual(1, previous.SetBit(7, true));
        Assert.AreEqual(2, previous.SetBit(6, true));
        Assert.AreEqual(4, previous.SetBit(5, true));
        Assert.AreEqual(8, previous.SetBit(4, true));
        Assert.AreEqual(16, previous.SetBit(3, true));
        Assert.AreEqual(32, previous.SetBit(2, true));
        Assert.AreEqual(64, previous.SetBit(1, true));
        Assert.AreEqual(128, previous.SetBit(0, true));
    }

    [Test]
    public void SetBitToTrueTest()
    {
        const byte previous = 0x00;

        Assert.AreEqual(1, previous.SetBit(7, true));
        Assert.AreEqual(2, previous.SetBit(6, true));
        Assert.AreEqual(4, previous.SetBit(5, true));
        Assert.AreEqual(8, previous.SetBit(4, true));
        Assert.AreEqual(16, previous.SetBit(3, true));
        Assert.AreEqual(32, previous.SetBit(2, true));
        Assert.AreEqual(64, previous.SetBit(1, true));
        Assert.AreEqual(128, previous.SetBit(0, true));
    }

    [Test]
    public void GetBitTest()
    {
        const byte testByte = 0xAA; // Binary = 1010 1010

        Assert.IsTrue(testByte.GetBit(0));
        Assert.IsFalse(testByte.GetBit(1));
        Assert.IsTrue(testByte.GetBit(2));
        Assert.IsFalse(testByte.GetBit(3));
        Assert.IsTrue(testByte.GetBit(4));
        Assert.IsFalse(testByte.GetBit(5));
        Assert.IsTrue(testByte.GetBit(6));
        Assert.IsFalse(testByte.GetBit(7));
    }

    [Test]
    public void ReadableStringTest()
    {
        var expected = new byte[] { 1 };
        const string readableByteArray = "1";
        var array = ByteArrayExtensions.FromReadableString(readableByteArray);
        Assert.AreEqual(expected, array);
    }
}
