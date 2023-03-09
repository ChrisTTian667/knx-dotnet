namespace Knx.KnxNetIp
{
    public enum KnxNetIpLayer
    {
        /// <summary>
        /// Tunneling on busmonitor layer, establishes a busmonitor tunnel to the KNX network.
        /// </summary>
        BusMonitor = 0x08,

        /// <summary>
        /// Tunneling on link layer, establishes a link layer tunnel to the KNX network.
        /// </summary>
        Link = 0x02,

        /// <summary>
        /// Tunneling on raw layer, establishes a raw tunnel to the KNX network.
        /// </summary>
        Raw = 0x04,
    }
}