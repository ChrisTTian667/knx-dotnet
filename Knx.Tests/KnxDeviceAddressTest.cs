﻿using System.Collections;
using Knx.Common;
using NUnit.Framework;

namespace Knx.Tests
{
    
    public class KnxDeviceAddressTest
    {
        [Test]
        public void KnxDeviceAddressByteArrayConstructorTest()
        {
            var bytes = new byte[] { 0x12, 0x03 };
            var target = new KnxDeviceAddress(bytes);

            Assert.AreEqual(target.Area, 1);
            Assert.AreEqual(target.Line, 2);
            Assert.AreEqual(target.Device, 3);
        }

        [Test]
        public void ToBitArrayTest()
        {
            var target = new KnxDeviceAddress(3, 2, 1); // 0x32, 0x01

            var expected = new BitArray(new[]
                                            {
                                                false, false, true, true,
                                                false, false, true, false,
                                                false, false, false, false, false, false, false, true,
                                            });
            BitArray actual = target.ToBitArray();

            Assert.AreEqual(expected.Length, actual.Length);

            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(actual[i], expected[i]);
        }
    }
}