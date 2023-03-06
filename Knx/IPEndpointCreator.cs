using System;
using System.Linq;
using System.Net;

namespace Knx;

public static class IPEndpointCreator
{
    public static bool IsValidIPEndPoint(string addressString)
    {
        if (string.IsNullOrWhiteSpace(addressString))
            return false;
        if (Uri.IsWellFormedUriString(addressString, UriKind.Absolute))
            return true;

        if (ParsePortNumber(addressString, out _))
            addressString = addressString.Substring(0, addressString.LastIndexOf(':'));

        return IPAddress.TryParse(addressString, out _);
    }

    public static IPEndPoint Create(string addressString)
    {
        if (ParsePortNumber(addressString, out var port))
            addressString = addressString.Substring(0, addressString.LastIndexOf(':'));

        IPEndPoint endPoint;

        if (Uri.IsWellFormedUriString(addressString, UriKind.Absolute))
            endPoint = ResolveHostName(addressString, port);
        else
        {
            // Normal IP Address or a DNS we need to resolve?
            IPAddress address = null;
            endPoint = IPAddress.TryParse(addressString, out address)
                ? new IPEndPoint(address, port)
                : ResolveHostName(addressString, port);
        }

        return endPoint;
    }

    private static IPEndPoint ResolveHostName(string addressString, int port)
    {
        var hostEntry = Dns.GetHostEntry(addressString);

        return new IPEndPoint(hostEntry.AddressList.First(), port);
    }

    private static bool ParsePortNumber(string addressString, out int port)
    {
        port = 3671;

        var lastDoublePointIndex = addressString.LastIndexOf(':');

        if (lastDoublePointIndex == -1)
            return false;

        try
        {
            port = Convert.ToUInt16(addressString.Substring(lastDoublePointIndex + 1));

            return true;
        }
        catch (FormatException)
        {
        }

        return false;
    }
}