using Knx.DatapointTypes;
using Knx.DatapointTypes.Dpt8BitSignedValue;
using NUnit.Framework;

namespace Knx.Tests
{
    
    public class DatapointTypes6XXXTests
    {
        [Test]
        public void DptPercentV8Test()
        {
            for (sbyte value = -128; value < 127; value++)
            {
                var dpt1 = new DptPercentV8(value);
                var dpt2 = new DptPercentV8(dpt1.Payload);

                Assert.AreEqual(dpt2.Value, value);
            }
        }

        [Test]
        public void DptValue1Count()
        {
            for (sbyte value = -128; value < 127; value++)
            {
                var dpt1 = new DptValue1Count(value);
                var dpt2 = new DptValue1Count(dpt1.Payload);

                Assert.AreEqual(dpt2.Value, value);
            }
        }

        [Test]
        public void DptStatusWithMode3Test()
        {
            var dpt1 = new DptStatusWithMode3(SetClear.Set, SetClear.Set, SetClear.Set, SetClear.Set, SetClear.Set, Mode3.Mode0);
            
            Assert.AreEqual(1, dpt1.Payload[0]);

            dpt1.A = SetClear.Clear;
            dpt1.B = SetClear.Clear;
            dpt1.C = SetClear.Clear;
            dpt1.D = SetClear.Clear;
            dpt1.E = SetClear.Clear;

            Assert.AreEqual(0xF9, dpt1.Payload[0]);
            
            dpt1.A = SetClear.Clear;
            dpt1.B = SetClear.Set;
            dpt1.C = SetClear.Clear;
            dpt1.D = SetClear.Set;
            dpt1.E = SetClear.Clear;
            dpt1.Mode = Mode3.Mode2;

            Assert.AreEqual(0xAC, dpt1.Payload[0]);
        }
    }
}
