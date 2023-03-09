using System;
using System.Collections.Generic;
using System.Linq;

namespace Knx
{
    /// <summary>
    ///     Eventargs for received bytes. As the .Net Framework does it.
    /// </summary>
    public class BytesReceivedEventArgs : EventArgs
    {
        #region Constructor / Destructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="BytesReceivedEventArgs" /> class.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public BytesReceivedEventArgs(IEnumerable<byte> bytes)
        {
            Bytes = bytes.ToArray();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the received bytes.
        /// </summary>
        public byte[] Bytes { get; }

        #endregion
    }
}