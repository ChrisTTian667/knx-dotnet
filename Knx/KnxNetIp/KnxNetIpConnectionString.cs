using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.KnxNetIp
{
    public sealed class KnxNetIpConnectionString
    {
        private KnxNetIpProtocol _externalProtocol;
        private KnxNetIpProtocol _internalProtocol;
        private TimeSpan _internalConnectionReleaseDelay;
        private TimeSpan _externalConnectionReleaseDelay;

        public KnxNetIpConnectionString()
        {
            InternalProtocol = KnxNetIpProtocol.Tunneling;
            ExternalProtocol = KnxNetIpProtocol.Tunneling;
            InternalMulticastUrl = Defaults.IPv4MulticastAddress.Address.ToString();
        }

        public KnxNetIpConnectionString(string connectionString) : this()
        {
            try
            {
                var str = connectionString.Trim('{', '}');
                var kvpairs = str.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var kv in kvpairs)
                {
                    if (kv.Contains("InternalUrl"))
                        InternalAddress = kv.Split('=')[1];
                    if (kv.Contains("ExternalUrl"))
                        ExternalAddress = kv.Split('=')[1];
                    if (kv.Contains("InternalProtocol"))
                        Enum.TryParse(kv.Split('=')[1], true, out _internalProtocol);
                    if (kv.Contains("ExternalProtocol"))
                        Enum.TryParse(kv.Split('=')[1], true, out _externalProtocol);
                    if (kv.Contains("InternalConnectionReleaseDelay"))
                        Enum.TryParse(kv.Split('=')[1], true, out _internalConnectionReleaseDelay);
                    if (kv.Contains("ExternalConnectionReleaseDelay"))
                        Enum.TryParse(kv.Split('=')[1], true, out _externalConnectionReleaseDelay);
                    if (kv.Contains("InternalMulticastUrl"))
                        InternalMulticastUrl = kv.Split('=')[1];
                    if (kv.Contains("DeviceAddress"))
                    {
                        var da = KnxAddress.ParseDevice(kv.Split('=')[1]);
                        DeviceMain = da.Area;
                        DeviceMiddle = da.Line;
                        DeviceSub = da.Device;
                    }
                }
            }
            catch (Exception exception)
            {
                throw new ArgumentException("ConnectionString is not formated correctly.", exception);
            }
        }

        public string InternalAddress { get; set; }

        public string InternalMulticastUrl { get; set; }

        public KnxNetIpProtocol InternalProtocol
        {
            get => _internalProtocol;
            set => _internalProtocol = value;
        }

        public KnxNetIpProtocol ExternalProtocol
        {
            get => _externalProtocol;
            set => _externalProtocol = value;
        }

        public TimeSpan InternalConnectionReleaseDelay
        {
            get => _internalConnectionReleaseDelay;
            set => _internalConnectionReleaseDelay = value;
        }

        public TimeSpan ExternalConnectionReleaseDelay
        {
            get => _externalConnectionReleaseDelay;
            set => _externalConnectionReleaseDelay = value;
        }

        public string ExternalAddress { get; set; }

        [Range(0, 15)]
        public byte DeviceMain { get; set; }

        [Range(0, 15)]
        public byte DeviceMiddle { get; set; }

        [Range(0, 255)]
        public byte DeviceSub { get; set; }

        public KnxDeviceAddress DeviceAddress => KnxAddress.Device(this.DeviceMain, this.DeviceMiddle, this.DeviceSub);

        public bool IsValid()
        {
            return IPEndpointCreator.IsValidIPEndPoint(InternalAddress)
                   || IPEndpointCreator.IsValidIPEndPoint(ExternalAddress);
        }

        public override string ToString()
        {
            return $"{{InternalUrl={InternalAddress}; ExternalUrl={ExternalAddress}; DeviceAddress={DeviceAddress}; InternalProtocol={InternalProtocol}; InternalConnectionReleaseDelay={InternalConnectionReleaseDelay}; ExternalProtocol={ExternalProtocol}; ExternalConnectionReleaseDelay={ExternalConnectionReleaseDelay}; InternalMulticastUrl={InternalMulticastUrl}}}";
        }
    }
}