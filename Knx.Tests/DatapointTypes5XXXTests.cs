using Knx.DatapointTypes.Dpt8BitUnsignedValue;
using NUnit.Framework;

namespace Knx.Tests
{
    
    public class DatapointTypes5XXXTests
    {
        [Test]
        public void DptScaleTests()
        {
            for (var i = 0; i <= 100; i++)
            {
                var dpt1 = new DptScaling(i);
                var dpt2 = new DptScaling(dpt1.Payload);

                Assert.IsFalse((i < dpt2.Value - 1) || (i > dpt2.Value + 1));
            }
        }

        [Test]
        public void DptAngleTests()
        {
            for (var i = 0; i <= 360; i++)
            {
                var dpt1 = new DptAngle(i);
                var dpt2 = new DptAngle(dpt1.Payload);

                Assert.IsFalse((i < dpt2.Value - 1) || (i > dpt2.Value + 1));
            }
        }

        [Test]
        public void DptDecimalFactorTests()
        {
            for (var i = 0; i <= 255; i++)
            {
                var dpt1 = new DptDecimalFactor(i);
                var dpt2 = new DptDecimalFactor(dpt1.Payload);

                Assert.IsFalse((i < dpt2.Value - 1) || (i > dpt2.Value + 1));
            }
        }

        [Test]
        public void DptPercentU8Tests()
        {
            for (var i = 0; i <= 255; i++)
            {
                var dpt1 = new DptPercentU8(i);
                var dpt2 = new DptPercentU8(dpt1.Payload);

                Assert.IsFalse((i < dpt2.Value - 1) || (i > dpt2.Value + 1));
            }
        }
    }
}
