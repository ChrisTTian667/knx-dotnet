using Knx.Common;
using Knx.DatapointTypes.DptString;

namespace Knx.KnxNetIp
{
    public class DeviceDescriptionInformationBlock : DescriptionInformationBlock
    {
        public KnxMedium Medium { get; protected set; }

        public short Status { get; protected set; }

        public byte[] MacAddress { get; protected set; }

        public string SerialNumber { get; protected set; }

        public int ProjectInstallId { get; protected set; }

        public string FriendlyName { get; protected set; }

        /// <summary>
        /// Gets the knx device address.
        /// </summary>
        public KnxDeviceAddress Address { get; protected set; }

        public static new DeviceDescriptionInformationBlock Parse(byte[] bytes)
        {
            return new DeviceDescriptionInformationBlock(bytes);
        }

        protected DeviceDescriptionInformationBlock(byte[] bytes) : base(bytes)
        {
            if (Type != DescriptionType.DeviceInfo)
                throw new KnxNetIpException("Unable to determine Device Description. Wrong Description Type!");

            Medium = (KnxMedium) Information[0];
            Status = Information[1];
            Address = new KnxDeviceAddress(Information.ExtractBytes(2, 2));
            ProjectInstallId = (Information[4] << 8) + Information[5];
            SerialNumber = new DptString_8859_1(Information.ExtractBytes(6, 6)).Value;
            MacAddress = Information.ExtractBytes(12, 10);
            FriendlyName = Resources.Strings.UnknownDevice;

            if (Information.Length > 22)
            {
                var name = new DptString_8859_1(Information.ExtractBytes(22)).Value;
                if (name.IndexOf('\0') >=0)
                    name = name.Substring(0, name.IndexOf('\0'));
                FriendlyName = name;
            }
        }
    }
}