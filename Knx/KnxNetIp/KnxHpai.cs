using System.Net;
using Knx.Common;

namespace Knx.KnxNetIp;

/// <summary>
///     KNXnet/IP Host Protocol Address Information (HPAI).
///     The address information is used to describe a communication channel. Its structure
///     varies according to the used underlying protocol. This class is implemented for IPv4.
///     For IP networks with NAT, consider use of {@link #HPAI(short, InetSocketAddress)}.
///     UDP is the default communication mode with mandatory support used in KNXnet/IP.
/// </summary>
public class KnxHpai
{
    public KnxHpai()
    {

    }

    private IPAddress _ipAddress;

    public HostProtocolCode HostProtocolCode { get; set; }

    public IPAddress IpAddress
    {
        get => _ipAddress ?? IPAddress.Loopback;
        set => _ipAddress = value;
    }

    public int Port { get; set; }

    public DeviceDescriptionInformationBlock? Description { get; private set; }

    /// <summary>
    ///     Parses the specified bytes.
    /// </summary>
    /// <param name="bytes">The bytes.</param>
    public static KnxHpai Parse(byte[] bytes)
    {
        var result = new KnxHpai
        {
            HostProtocolCode = (HostProtocolCode)bytes[1],
            IpAddress = new IPAddress(bytes.ExtractBytes(2, 4)),
            Port = (bytes[6] << 8) + bytes[7]
        };
        try
        {
            if (bytes.Length > 8)
                result.Description =
                    DeviceDescriptionInformationBlock.Parse(bytes.ExtractBytes(8));
        }
        catch
        {
            // suppress any exceptions.
        }

        return result;
    }

    public byte[] ToByteArray()
    {
        var arrayBuilder =
            new ByteArrayBuilder()
                .AddToken(1, out var lengthToken)
                .AddByte((byte)HostProtocolCode)
                .Add(IpAddress)
                .AddInt(Port);

        arrayBuilder.ReplaceToken(lengthToken, arrayBuilder.Length);

        return arrayBuilder.ToByteArray();
    }
}
