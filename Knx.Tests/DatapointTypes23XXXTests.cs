using Knx.DatapointTypes.Dpt2BitEnumeration;
using NUnit.Framework;

namespace Knx.Tests
{
    
    public class DatapointTypes23XXXTests
    {
        [Test]
        public void DptOnOffActionTest()
        {
            var dpt1 = new DptOnOffAction(OnOffAction.OffOn);
            var dpt2 = new DptOnOffAction(dpt1.Payload);

            Assert.AreEqual(OnOffAction.OffOn, dpt2.Value);
        }

        [Test]
        public void DptAlaramReactionTest()
        {
            var dpt1 = new DptAlarmReaction(AlarmReaction.AlarmPositionUp);
            var dpt2 = new DptAlarmReaction(dpt1.Payload);

            Assert.AreEqual(AlarmReaction.AlarmPositionUp, dpt2.Value);
        }

        [Test]
        public void DptUpDownActionTest()
        {
            var dpt1 = new DptUpDownAction(UpDownAction.Down);
            var dpt2 = new DptUpDownAction(dpt1.Payload);

            Assert.AreEqual(UpDownAction.Down, dpt2.Value);
        }
    }
}
