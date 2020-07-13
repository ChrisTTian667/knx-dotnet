using System;
using System.Net;
using System.Threading;

namespace Knx
{
    /// <summary>
    ///     General interface for all udp clients.
    /// </summary>
    public interface IUdpClient : IDisposable
    {
        /// <summary>
        ///     Gets the local endpoint.
        /// </summary>
        IPEndPoint LocalEndpoint { get; }

        /// <summary>
        ///     Gets or sets the remote endpoint.
        /// </summary>
        IPEndPoint RemoteEndpoint { get; }

        /// <summary>
        /// Connects to the specified endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        void Connect(IPEndPoint endpoint);

        /// <summary>
        ///     Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        void Send(byte[] data);

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="length">The length.</param>
        void Send(byte[] data, int length);

        byte[] Receive();

        byte[] Receive(WaitHandle stopHandle);

        /// <summary>
        /// Closes the udp connection.
        /// </summary>
        void Close();
    }
}