using Knx.DatapointTypes.Dpt8BitCharackter;
using NUnit.Framework;

namespace Knx.Tests
{
    
    public class DatapointTypes4XXXTests
    {
        [Test]
        public void EncodeAsciiCharackter()
        {
            const char charackter = 'A';

            var datapointType = new DptCharackterAscii(charackter);

            Assert.AreEqual(0x41, datapointType.Payload[0]);

            var datapointType2 = new DptCharackterAscii(datapointType.Payload);

            Assert.AreEqual('A', datapointType2.Value);
        }

        [Test]
        public void EncodeAndDecodeAsciiCharackters()
        {
            for (int i = 0; i < 127; i++)
            {
                var charackter = ((char) i);

                var dpt1 = new DptCharackterAscii(charackter);
                var dpt2 = new DptCharackterAscii(dpt1.Payload);

                Assert.AreEqual(dpt2.Value, charackter);
            }
        }

        [Test]
        public void Encode8859_1Charackter()
        {
            const char charackter = 'A';
            var dpt = new DptCharackter_8859_1(charackter);
            Assert.AreEqual(0x41, dpt.Payload[0]);
        }

        [Test]
        public void EncodeAndDecode8859_1Charackters()
        {
            for (int i = 0; i < 255; i++)
            {
                var charackter = ((char)i);

                var dpt1 = new DptCharackter_8859_1(charackter);
                var dpt2 = new DptCharackter_8859_1(dpt1.Payload);

                Assert.AreEqual(dpt2.Value, charackter);
            }
        }
    }
}
