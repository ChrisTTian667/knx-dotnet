using System;
using Knx.DatapointTypes.Dpt4ByteSignedValue;
using NUnit.Framework;

namespace Knx.Tests
{
    
    public class DatapointTypes13XXXTests
    {
        [Test]
        public void DptValue4CountTest()
        {
            var dpt1 = new DptValue4Count(int.MaxValue);
            var dpt2 = new DptValue4Count(dpt1.Payload);
            var dpt3 = new DptValue4Count(int.MinValue);
            var dpt4 = new DptValue4Count(dpt3.Payload);

            Assert.AreEqual(dpt2.Value, int.MaxValue);
            Assert.AreEqual(dpt4.Value, int.MinValue);
        }

        [Test]
        public void DptLongDeltaTimeSecTest()
        {
            var dpt1 = new DptLongDeltaTimeSec(TimeSpan.FromSeconds(int.MinValue));
            var dpt2 = new DptLongDeltaTimeSec(dpt1.Payload);
            var dpt3 = new DptLongDeltaTimeSec(TimeSpan.FromSeconds(int.MaxValue));
            var dpt4 = new DptLongDeltaTimeSec(dpt3.Payload);

            Assert.AreEqual(dpt2.Value, TimeSpan.FromSeconds(int.MinValue));
            Assert.AreEqual(dpt4.Value, TimeSpan.FromSeconds(int.MaxValue));
        }
    }
}
