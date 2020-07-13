using Knx.DatapointTypes.Dpt8BitBitset;
using NUnit.Framework;

namespace Knx.Tests
{
    public class DatapointTypes21XXXTests
    {
        [Test]
        public void DptStatusGenTest()
        {
            var dpt1 = new DptGeneralStatus(false, true, false, true, false);
            var dpt2 = new DptGeneralStatus(dpt1.Payload);

            Assert.IsFalse(dpt2.OutOfService);
            Assert.IsTrue(dpt2.Fault);
            Assert.IsFalse(dpt2.Overridden);
            Assert.IsTrue(dpt2.InAlarm);
            Assert.IsFalse(dpt2.AlarmUnacknowledged);
        }

        [Test]
        public void DptDeviceControlTest()
        {
            var dpt1 = new DptDeviceControl(true, false, true);
            var dpt2 = new DptDeviceControl(dpt1.Payload);

            Assert.IsTrue(dpt2.UserStopped);
            Assert.IsFalse(dpt2.DatagramWithOwnIndividualAddressReceived);
            Assert.IsTrue(dpt2.VerifyMode);
            Assert.AreEqual(0x05, dpt2.Payload[0]);
        }
    }
}
