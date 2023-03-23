using System;
using System.Collections.Generic;
using System.Linq;

namespace Knx;

/// <summary>
///     Event arguments for received bytes
/// </summary>
public class BytesReceivedEventArgs : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="BytesReceivedEventArgs" /> class.
    /// </summary>
    /// <param name="bytes">The bytes.</param>
    public BytesReceivedEventArgs(IEnumerable<byte> bytes)
    {
        Bytes = bytes.ToArray();
    }

    /// <summary>
    ///     Gets the received bytes.
    /// </summary>
    public byte[] Bytes { get; }
}
