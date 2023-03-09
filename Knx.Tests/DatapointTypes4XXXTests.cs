using Knx.DatapointTypes.Dpt8BitCharackter;
using NUnit.Framework;

namespace Knx.Tests;

public class DatapointTypes4XXXTests
{
    [Test]
    public void EncodeAsciiCharacter()
    {
        const char character = 'A';

        var datapointType = new DptCharackterAscii(character);

        Assert.AreEqual(0x41, datapointType.Payload[0]);

        var datapointType2 = new DptCharackterAscii(datapointType.Payload);

        Assert.AreEqual('A', datapointType2.Value);
    }

    [Test]
    public void EncodeAndDecodeAsciiCharacters()
    {
        for (var i = 0; i < 127; i++)
        {
            var character = (char)i;

            var dpt1 = new DptCharackterAscii(character);
            var dpt2 = new DptCharackterAscii(dpt1.Payload);

            Assert.AreEqual(dpt2.Value, character);
        }
    }

    [Test]
    public void Encode8859_1Character()
    {
        const char character = 'A';
        var dpt = new DptCharacter_8859_1(character);
        Assert.AreEqual(0x41, dpt.Payload[0]);
    }

    [Test]
    public void EncodeAndDecode8859_1Characters()
    {
        for (var i = 0; i < 255; i++)
        {
            var character = (char)i;

            var dpt1 = new DptCharacter_8859_1(character);
            var dpt2 = new DptCharacter_8859_1(dpt1.Payload);

            Assert.AreEqual(dpt2.Value, character);
        }
    }
}
