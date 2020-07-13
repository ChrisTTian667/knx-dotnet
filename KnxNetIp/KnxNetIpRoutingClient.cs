using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Knx.Common;
using Knx.KnxNetIp.MessageBody;
using PortableDI;

namespace Knx.KnxNetIp
{
        /// <summary>
    ///     KnxNetIp Client for UDP Multicast usage
    /// </summary>
    public class KnxNetIpRoutingClient : IKnxClient, IDisposable
    {
        private IMulticastUdpClient _udpClient;

        public KnxNetIpRoutingClient()
        {
            // default
            RemoteEndpoint = new IPEndPoint(IPAddress.Parse("224.0.23.12"), 3671);
            DeviceAddress = new KnxDeviceAddress(0,0,0);
            ReadTimeout = TimeSpan.FromSeconds(3);
        }

        public KnxNetIpRoutingClient(IPEndPoint remoteEndPoint, KnxDeviceAddress deviceAddress)
        {
            RemoteEndpoint = remoteEndPoint;
            DeviceAddress = deviceAddress;
            ReadTimeout = TimeSpan.FromSeconds(3);
        }

        #region IKnxClient Members

        /// <summary>
        /// Occurs when [KNX message received].
        /// </summary>
        public event KnxMessageReceivedHandler KnxMessageReceived;

        public IPEndPoint RemoteEndpoint { get; private set; }

        /// <summary>
        /// The DeviceAddress of the KNX Gateway
        /// </summary>
        public KnxDeviceAddress DeviceAddress { get; private set; }

        /// <summary>
        /// Gets the read timeout.
        /// </summary>
        public TimeSpan ReadTimeout { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the client is connected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if client is connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Connects this instance.
        /// </summary>
        public void Open()
        {
            if (_udpClient != null)
            {
                _udpClient.Dispose();
                _udpClient = null;
            }

            _udpClient = CreateUdpClient(RemoteEndpoint);
            Initialize();

            IsConnected = true;
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="knxMessage">The KNX message.</param>
        public void SendMessage(IKnxMessage knxMessage)
        {
            if (_udpClient == null)
                Open();
        
            // create net ip message
            KnxNetIpMessage netIpMessage = KnxNetIpMessage.Create(KnxNetIpServiceType.RoutingIndication);

            // set knx message
            ((RoutingIndication) netIpMessage.Body).Cemi = knxMessage;

            // send
            Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} SEND => {netIpMessage}");
            _udpClient.Send(netIpMessage.ToByteArray());
        }

        #endregion

        private void Initialize()
        {
            _udpClient.Received += (sender, args) =>
                                       {
                                           IsConnected = true;

                                           var msg = KnxNetIpMessage.Parse(args.Bytes);
                                           if (msg == null)
                                               return;

                                           Debug.WriteLine("{0} RECV <= {1} (HANDLED)", DateTime.Now.ToLongTimeString(), msg.ToString());

                                           var routingIndication = msg.Body as RoutingIndication;
                                           if (routingIndication == null)
                                               return;

                                           OnKnxMessageReceived(routingIndication.Cemi);
                                       };
        }

        private IMulticastUdpClient CreateUdpClient(IPEndPoint endpoint)
        {
            var udpClient = DIContainer.Resolve<IMulticastUdpClient>();
            udpClient.Connect(endpoint);

            return udpClient;
        }

        /// <summary>
        /// Called when [KNX message received].
        /// </summary>
        /// <param name="message">The message.</param>
        protected virtual void OnKnxMessageReceived(IKnxMessage message)
        {
            var handler = KnxMessageReceived;
            if (handler != null)
                handler(this, message);
        }

        #region IDisposable

        ~KnxNetIpRoutingClient()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_udpClient != null)
                {
                    _udpClient.Dispose();
                    _udpClient = null;
                }
            }
        }

        #endregion
    }
    
    
    // /// <summary>
    // ///     KnxNetIp Client for UDP Multicast usage
    // /// </summary>
    // public class KnxNetIpRoutingClient : IKnxClient, IDisposable
    // {
    //     private UdpClient _udpClient;
    //
    //     public KnxNetIpRoutingClient()
    //     {
    //         // default
    //         RemoteEndpoint = new IPEndPoint(IPAddress.Parse("224.0.23.12"), 3671);
    //         DeviceAddress = new KnxDeviceAddress(0,0,0);
    //         ReadTimeout = TimeSpan.FromSeconds(3);
    //     }
    //
    //     public KnxNetIpRoutingClient(IPEndPoint remoteEndPoint, KnxDeviceAddress deviceAddress)
    //     {
    //         RemoteEndpoint = remoteEndPoint;
    //         DeviceAddress = deviceAddress;
    //         ReadTimeout = TimeSpan.FromSeconds(3);
    //     }
    //
    //     #region IKnxClient Members
    //
    //     /// <summary>
    //     /// Occurs when [KNX message received].
    //     /// </summary>
    //     public event KnxMessageReceivedHandler KnxMessageReceived;
    //
    //     public IPEndPoint RemoteEndpoint { get; private set; }
    //
    //     /// <summary>
    //     /// The DeviceAddress of the KNX Gateway
    //     /// </summary>
    //     public KnxDeviceAddress DeviceAddress { get; private set; }
    //
    //     /// <summary>
    //     /// Gets the read timeout.
    //     /// </summary>
    //     public TimeSpan ReadTimeout { get; private set; }
    //
    //     /// <summary>
    //     /// Gets a value indicating whether the client is connected.
    //     /// </summary>
    //     /// <value>
    //     ///   <c>true</c> if client is connected; otherwise, <c>false</c>.
    //     /// </value>
    //     public bool IsConnected { get; private set; }
    //
    //     /// <summary>
    //     /// Connects this instance.
    //     /// </summary>
    //     public void Open()
    //     {
    //         if (_udpClient != null)
    //         {
    //             _udpClient.Dispose();
    //             _udpClient = null;
    //         }
    //
    //         _udpClient = new UdpClient(RemoteEndpoint);
    //         _udpClient.BeginReceive(OnReceive, null);
    //
    //         IsConnected = true;
    //     }
    //
    //     private void OnReceive(IAsyncResult asyncResult)
    //     {
    //         var remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 8000);
    //         byte[] received = _udpClient.EndReceive(asyncResult, ref remoteIpEndPoint);
    //         _udpClient.BeginReceive(new AsyncCallback(OnReceive), null);
    //         
    //         IsConnected = true;
    //
    //         var msg = KnxNetIpMessage.Parse(received);
    //         if (msg == null)
    //             return;
    //
    //         Debug.WriteLine("{0} RECV <= {1} (HANDLED)", DateTime.Now.ToLongTimeString(), msg.ToString());
    //
    //         var routingIndication = msg.Body as RoutingIndication;
    //         if (routingIndication == null)
    //             return;
    //
    //         OnKnxMessageReceived(routingIndication.Cemi);
    //     }
    //
    //     /// <summary>
    //     /// Sends the message.
    //     /// </summary>
    //     /// <param name="knxMessage">The KNX message.</param>
    //     public void SendMessage(IKnxMessage knxMessage)
    //     {
    //         if (_udpClient == null)
    //             Open();
    //     
    //         // create net ip message & set cemi
    //         KnxNetIpMessage netIpMessage = KnxNetIpMessage.Create(KnxNetIpServiceType.RoutingIndication);
    //         ((RoutingIndication) netIpMessage.Body).Cemi = knxMessage;
    //
    //         // send
    //         Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} SEND => {netIpMessage}");
    //
    //         var byteArray = netIpMessage.ToByteArray();
    //         await _udpClient.SendAsync(byteArray, byteArray.Length);
    //     }
    //
    //     #endregion
    //
    //     /// <summary>
    //     /// Called when [KNX message received].
    //     /// </summary>
    //     /// <param name="message">The message.</param>
    //     protected virtual void OnKnxMessageReceived(IKnxMessage message)
    //     {
    //         var handler = KnxMessageReceived;
    //         if (handler != null)
    //             handler(this, message);
    //     }
    //
    //     #region IDisposable
    //
    //     ~KnxNetIpRoutingClient()
    //     {
    //         Dispose(false);
    //     }
    //
    //     public void Dispose()
    //     {
    //         Dispose(true);
    //         GC.SuppressFinalize(this);
    //     }
    //
    //     private void Dispose(bool disposing)
    //     {
    //         if (disposing)
    //         {
    //             if (_udpClient != null)
    //             {
    //                 _udpClient.Dispose();
    //                 _udpClient = null;
    //             }
    //         }
    //     }
    //
    //     #endregion
    // }
}