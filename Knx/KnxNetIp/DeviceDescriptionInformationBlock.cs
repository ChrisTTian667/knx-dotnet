using Knx.Common;
using Knx.DatapointTypes.DptString;
using Knx.Resources;

namespace Knx.KnxNetIp;

public sealed class DeviceDescriptionInformationBlock : DescriptionInformationBlock
{
    private DeviceDescriptionInformationBlock(byte[] bytes) : base(bytes)
    {
        Medium = (KnxMedium)Information[0];
        Status = Information[1];
        Address = new KnxDeviceAddress(Information.ExtractBytes(2, 2));
        ProjectInstallId = (Information[4] << 8) + Information[5];
        SerialNumber = new DptString_8859_1(Information.ExtractBytes(6, 6)).Value;
        MacAddress = Information.ExtractBytes(16, 6);
        FriendlyName = Strings.UnknownDevice;

        if (Information.Length > 22)
        {
            var name = new DptString_8859_1(Information.ExtractBytes(22)).Value;
            if (name.Contains('\0'))
                name = name[..name.IndexOf('\0')];

            FriendlyName = name;
        }

        if (Type != DescriptionType.DeviceInfo)
            throw new KnxNetIpException("Unable to determine Device Description. Wrong Description Type!");
    }

    public KnxMedium Medium { get; }

    public short Status { get; }

    public byte[] MacAddress { get; }

    public string SerialNumber { get; }

    public int ProjectInstallId { get; }

    public string FriendlyName { get; }

    public KnxDeviceAddress Address { get; }

    public new static DeviceDescriptionInformationBlock Parse(byte[] bytes)
    {
        return new(bytes);
    }
}
