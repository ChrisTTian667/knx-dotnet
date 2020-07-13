using System.Net;

namespace Knx
{
    public static class Defaults
    {
        public static readonly IPEndPoint IPv4MulticastAddress = new IPEndPoint(IPAddress.Parse("224.0.23.12"), 3671);
    }
}
