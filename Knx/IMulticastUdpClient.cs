using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Knx
{
    public interface IMulticastUdpClient 
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
        Task Send(byte[] data);

        byte[] Receive();

        byte[] Receive(WaitHandle stopHandle);

        /// <summary>
        /// Closes the udp connection.
        /// </summary>
        void Close();
        
        event EventHandler<BytesReceivedEventArgs> Received;
    }
}