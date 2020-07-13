using System.Net;
using Knx.KnxNetIp;
using NUnit.Framework;

namespace Knx.Tests
{
    
    public class KnxHpaiTests
    {
        [Test]
        public void CreateKnxHpai()
        {
            var hpai = new KnxHpai
            {
                HostProtocolCode = HostProtocolCode.IPV4_UDP,
                IpAddress = IPAddress.Parse("192.168.2.1"),
                Port = 3060
            };

            Assert.IsNotNull(hpai);
        }

        [Test]
        public void SerializeDeserializeKnxHpai()
        {
            var hpai = new KnxHpai
            {
                HostProtocolCode = HostProtocolCode.IPV4_UDP,
                IpAddress = IPAddress.Parse("192.168.2.1"),
                Port = 3060
            };

            var array = hpai.ToByteArray();
            var deserializedHpai = KnxHpai.Parse(array);

            Assert.AreEqual(hpai.HostProtocolCode, deserializedHpai.HostProtocolCode);
            Assert.AreEqual(hpai.IpAddress, deserializedHpai.IpAddress);
            Assert.AreEqual(hpai.Port, deserializedHpai.Port);
        }
    }
}
