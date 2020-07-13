using Knx.DatapointTypes.Dpt8BitEnumeration;
using NUnit.Framework;

namespace Knx.Tests
{
    
    public class DatapointTypes20XXXTests
    {
        [Test]
        public void DptSCLOModeTest()
        {
            var dpt1 = new DptSCLOMode(SCLOMode.Master);
            var dpt2 = new DptSCLOMode(dpt1.Payload);

            Assert.AreEqual(SCLOMode.Master, dpt2.Value);
        }

        [Test]
        public void Dpt8BitEnumValidationTest()
        {
            var dpt1 = new DptSCLOMode(new [] { byte.MaxValue });
            var dpt2 = new DptSCLOMode(new byte[] { 0 });

            Assert.IsFalse(dpt1.IsValueValid);
            Assert.IsTrue(dpt2.IsValueValid);
        }
    }
}
