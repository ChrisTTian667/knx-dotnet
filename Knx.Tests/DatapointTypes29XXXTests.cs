using System;
using Knx.DatapointTypes.Dpt8ByteSignedValue;
using NUnit.Framework;

namespace Knx.Tests
{
    
    public class DatapointTypes29XXXTests
    {
        [Test]
        public void DptActiveEnergy64Test()
        {
            var dpt1 = new DptActiveEnergy64(Int64.MaxValue);
            var dpt2 = new DptActiveEnergy64(dpt1.Payload);

            var dpt3 = new DptActiveEnergy64(Int64.MinValue);
            var dpt4 = new DptActiveEnergy64(dpt3.Payload);

            Assert.AreEqual(dpt2.Value, Int64.MaxValue);
            Assert.AreEqual(dpt4.Value, Int64.MinValue);
        }
    
        [Test]
        public void DptApparantEnergy64Test()
        {
            var dpt1 = new DptApparantEnergy64(Int64.MaxValue);
            var dpt2 = new DptApparantEnergy64(dpt1.Payload);

            var dpt3 = new DptApparantEnergy64(Int64.MinValue);
            var dpt4 = new DptApparantEnergy64(dpt3.Payload);

            Assert.AreEqual(dpt2.Value, Int64.MaxValue);
            Assert.AreEqual(dpt4.Value, Int64.MinValue);
        }

        [Test]
        public void DptReactiveEnergy64Test()
        {
            var dpt1 = new DptReactiveEnergy64(Int64.MaxValue);
            var dpt2 = new DptReactiveEnergy64(dpt1.Payload);

            var dpt3 = new DptReactiveEnergy64(Int64.MinValue);
            var dpt4 = new DptReactiveEnergy64(dpt3.Payload);

            Assert.AreEqual(dpt2.Value, Int64.MaxValue);
            Assert.AreEqual(dpt4.Value, Int64.MinValue);
        }
    }
}
