using Knx.Common;
using NUnit.Framework;

namespace Knx.Tests;

/// <summary>
///     This is a test class for ByteArrayExtensionsTest and is intended
///     to contain all ByteArrayExtensionsTest Unit Tests
/// </summary>
public class ByteArrayExtensionsTest
{
    [Test]
    public void SetBitAtPositionTest()
    {
        var array = new byte[] { 3 };
        array.SetBitAtPosition(7, false);
        array.SetBitAtPosition(5, true);
        array.SetBitAtPosition(4, true);

        Assert.AreEqual((byte)14, array[0]);
    }
}
