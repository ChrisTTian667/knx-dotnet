using System;
using System.Collections;
using Knx.Common;
using NUnit.Framework;

namespace Knx.Tests
{
    /// <summary>
    ///This is a test class for KnxLogicalAddressTest and is intended
    ///to contain all KnxLogicalAddressTest Unit Tests
    ///</summary>
    
    public class KnxLogicalAddressTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes

        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion

        /// <summary>
        ///A test for KnxLogicalAddress Constructor
        ///</summary>
        [Test]
        public void KnxLogicalAddressByteArrayConstructorTest()
        {
            var data = new byte[] { 0x1A, 0x07 };
            var target = new KnxLogicalAddress(data);

            Assert.AreEqual(target.Group, 3);
            Assert.AreEqual(target.MiddleGroup, new byte?(2));
            Assert.AreEqual(target.SubGroup, 7);
        }

        /// <summary>
        ///A test for ToBitArray
        ///</summary>
        [Test]
        public void ToBitArrayWithMiddleGroupTest()
        {
            var target = new KnxLogicalAddress(3, 2, 7);

            var expected = new BitArray(new[]
                                            {
                                                false, // reserved bit
                                                false, false, true, true, // 4 bits for area
                                                false, true, false, // 3 bits for middle group
                                                false, false, false, false, false, true, true, true,
                                                // 8 bits for sub group
                                            });
            BitArray actual = target.ToBitArray();

            Assert.AreEqual(expected.Length, actual.Length);

            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(actual[i], expected[i]);
        }

        /// <summary>
        ///A test for ToBitArray
        ///</summary>
        [Test]
        public void ToBitArrayWithoutMiddleGroupTest()
        {
            var target = new KnxLogicalAddress(3, 12);

            var expected = new BitArray(new[]
                                            {
                                                false, // reserved bit
                                                false, false, true, true, // 4 bits for area
                                                false, false, false, false, false, false, false, true, true, false,
                                                false, // 11 bits for sub group
                                            });
            BitArray actual = target.ToBitArray();

            Assert.AreEqual(expected.Length, actual.Length);

            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(actual[i], expected[i]);
        }

        [Test]
        public void ParseAddressTest()
        {
            var address = KnxAddress.ParseLogical("1/2/3");
            Assert.IsNotNull(address);

            address = KnxAddress.ParseLogical("15-7/3");
            Assert.IsNotNull(address);

            address = KnxAddress.ParseLogical("15-7-255");
            Assert.IsNotNull(address);

            try
            {
                address = KnxAddress.ParseLogical("hello world");
                Assert.Fail("Exception should be trown.");
            }
            catch (FormatException)
            {
            }
        }
    }
}