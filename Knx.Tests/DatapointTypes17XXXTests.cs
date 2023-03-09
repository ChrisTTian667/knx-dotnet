using Knx.DatapointTypes;
using NUnit.Framework;

namespace Knx.Tests
{
    
    public class DatapointTypes17XXXTests
    {
        [Test]
        public void DptSceneTest()
        {
            var dpt1 = new DptScene(17);
            var dpt2 = new DptScene(dpt1.Payload);

            Assert.AreEqual(17, dpt2.Scene);
        }
    }
}
