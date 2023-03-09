using System.Net;

namespace Knx;

public static class Default
{
    // ReSharper disable once InconsistentNaming
    public static readonly IPEndPoint IPv4MulticastAddress =
        new(IPAddress.Parse("224.0.23.12"), 3671);
}
