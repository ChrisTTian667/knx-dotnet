using System;

namespace Knx.Common
{
    [Flags]
    public enum MessageType
    {
        /// <summary>
        /// Read from Bus
        /// </summary>
        Read = 0x00,

        /// <summary>
        /// Reply to Bus
        /// </summary>
        Reply = 0x40,

        /// <summary>
        /// Write to Bus
        /// </summary>
        Write = 0x80
    }
}
