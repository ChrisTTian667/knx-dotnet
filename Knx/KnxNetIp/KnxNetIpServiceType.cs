namespace Knx.KnxNetIp
{
    public enum KnxNetIpServiceType
    {
        /**
     * Service type identifier for a connect request.
     * <p>
     */
        ConnectionRequest = 0x0205,

        /**
         * Service type identifier for a connect response.
         * <p>
         */
        ConnectionResponse = 0x0206,

        /**
         * Service type identifier for a connection state request.
         * <p>
         */
        ConnectionStateRequest = 0x0207,

        /**
         * Service type identifier for a connection state response.
         * <p>
         */
        ConnectionStateResponse = 0x0208,

        /**
         * Service type identifier for a disconnect request.
         * <p>
         */
        DisconnectRequest = 0x0209,

        /**
         * Service type identifier for a disconnect response.
         * <p>
         */
        DisconnectResponse = 0x020A,

        /**
         * Service type identifier for a description request.
         * <p>
         */
        DescriptionRequest = 0x0203,

        /**
         * Service type identifier for a description response.
         * <p>
         */
        DescriptionResponse = 0x204,

        /**
         * Service type identifier for a search request.
         * <p>
         */
        SearchRequest = 0x201,

        /**
         * Service type identifier for a search response.
         * <p>
         */
        SearchResponse = 0x202,

        /**
         * Service type identifier for configuration request (read / write device
         * configuration data, interface properties).
         * <p>
         */
        DeviceConfigurationRequest = 0x0310,

        /**
         * Service type identifier to confirm the reception of the configuration request.
         * <p>
         */
        DeviceConfigurationAcknowledge = 0x0311,

        /**
         * Service type identifier to send and receive single KNX frames between client and
         * server.
         * <p>
         */
        TunnelingRequest = 0x0420,

        /**
         * Service type identifier to confirm the reception of a tunneling request.
         * <p>
         */
        TunnelingAcknowledge = 0x0421,

        /**
         * Service type identifier for sending KNX telegrams over IP networks with multicast.
         * <p>
         */
        RoutingIndication = 0x0530,

        /**
         * Service type identifier to indicate the loss of routing messages with multicast.
         * <p>
         */
        RoutingLostMessage = 0x0531,
    }
}