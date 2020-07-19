namespace Knx
{
    public enum TransportLayerControlInfo
    {
        /// <summary>
        /// UDT - e.g. GroupMessages
        /// </summary>
        UnnumberedDataPacket = 0x00,

        /// <summary>
        /// NDT - e.g. Transmits the Data
        /// </summary>
        NumberedDataPacket = 0x01,

        /// <summary>
        /// UCD - OPR & CLR
        /// </summary>
        UnnumberedControlData = 0x02,

        /// <summary>
        /// NCD - e.g. to commit the data transfer (ACK & NAK)
        /// </summary>
        NumberedControlData = 0x03
    }
}