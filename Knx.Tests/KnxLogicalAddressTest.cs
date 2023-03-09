using System;
using System.Collections;
using NUnit.Framework;

namespace Knx.Tests;

/// <summary>
///     This is a test class for KnxLogicalAddressTest and is intended
///     to contain all KnxLogicalAddressTest Unit Tests
/// </summary>
public class KnxLogicalAddressTest
{
    /// <summary>
    ///     A test for KnxLogicalAddress Constructor
    /// </summary>
    [Test]
    public void KnxLogicalAddressByteArrayConstructorTest()
    {
        var data = new byte[] { 0x1A, 0x07 };
        var target = new KnxLogicalAddress(data);

        Assert.AreEqual(target.Group, 3);
        Assert.AreEqual(target.MiddleGroup, 2);
        Assert.AreEqual(target.SubGroup, 7);
    }

    /// <summary>
    ///     A test for ToBitArray
    /// </summary>
    [Test]
    public void ToBitArrayWithMiddleGroupTest()
    {
        var target = new KnxLogicalAddress(3, 2, 7);

        var expected = new BitArray(
            new[]
            {
                false, // reserved bit
                false, false, true, true, // 4 bits for area
                false, true, false, // 3 bits for middle group
                false, false, false, false, false, true, true, true
                // 8 bits for sub group
            });
        var actual = target.ToBitArray();

        Assert.AreEqual(expected.Length, actual.Length);

        for (var i = 0; i < actual.Length; i++)
            Assert.AreEqual(actual[i], expected[i]);
    }

    /// <summary>
    ///     A test for ToBitArray
    /// </summary>
    [Test]
    public void ToBitArrayWithoutMiddleGroupTest()
    {
        var target = new KnxLogicalAddress(3, 12);

        var expected = new BitArray(
            new[]
            {
                false, // reserved bit
                false, false, true, true, // 4 bits for area
                false, false, false, false, false, false, false, true, true, false,
                false // 11 bits for sub group
            });
        var actual = target.ToBitArray();

        Assert.AreEqual(expected.Length, actual.Length);

        for (var i = 0; i < actual.Length; i++)
            Assert.AreEqual(actual[i], expected[i]);
    }

    [Test]
    public void ParseAddressTest()
    {
        var address = KnxAddress.ParseLogical("1/2/3");
        Assert.AreEqual(1, address.Group);
        Assert.AreEqual(2, address.MiddleGroup);
        Assert.AreEqual(3, address.SubGroup);

        address = KnxAddress.ParseLogical("15-7/3");
        Assert.AreEqual(15, address.Group);
        Assert.AreEqual(7, address.MiddleGroup);
        Assert.AreEqual(3, address.SubGroup);

        address = KnxAddress.ParseLogical("15-7-255");
        Assert.AreEqual(15, address.Group);
        Assert.AreEqual(7, address.MiddleGroup);
        Assert.AreEqual(255, address.SubGroup);

        Assert.Throws<FormatException>(() => KnxAddress.ParseLogical("hello world"));
    }
}
