using System;
using System.Net;
using Knx.Common;

namespace Knx.KnxNetIp;

/// <summary>
/// The KNX Host Protocol Address Information (HPAI) is a component
/// of the KNXnet/IP communication protocol, responsible for encoding
/// and decoding the address information of the knx devices.
/// </summary>
public class KnxHpai
{
    private IPAddress? _ipAddress;

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
    public static KnxHpai Parse(byte[] bytes)
    {
        var result = new KnxHpai
        {
            HostProtocolCode = (HostProtocolCode)bytes[1],
            IpAddress = new IPAddress(bytes.ExtractBytes(2, 4)),
            Port = (bytes[6] << 8) + bytes[7]
        };

        if (bytes.Length > 8)
        {
            result.Description =
                DeviceDescriptionInformationBlock.Parse(bytes.ExtractBytes(8));
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

        return arrayBuilder
            .ReplaceToken(lengthToken, arrayBuilder.Length)
            .ToByteArray();
    }
}
