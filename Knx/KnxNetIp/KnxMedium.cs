namespace Knx.KnxNetIp
{
    public enum KnxMedium
    {
        /// <summary>
        /// KNX medium code for twisted pair 0 (2400 bit/s), inherited from BatiBUS. (TP0)
        /// </summary>
        TwistedPair2400Baud = 0x01,

        /// <summary>
        /// KNX medium code for twisted pair 1 (9600 bit/s). (TP1)
        /// </summary>
        TwistedPair9600Baud = 0x02,

        /// <summary>
        /// KNX medium code for power line 110 kHz (1200 bit/s).
        /// </summary>
        Powerline110kHz = 0x04,

        /// <summary>
        /// KNX medium code for power line 132 kHz (2400 bit/s), inherited from EHS.
        /// </summary>
        Powerline132kHz = 0x08,

        /// <summary>
        /// KNX medium code for radio frequency (868 MHz).
        /// </summary>
        RadioFrequency = 0x10
    }
}