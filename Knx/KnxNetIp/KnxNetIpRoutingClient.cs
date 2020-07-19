using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Knx.KnxNetIp.MessageBody;

namespace Knx.KnxNetIp
{
    /// <summary>
    /// Used to connect to the Knx Bus via KnxNetIpRouting protocol.
    /// </summary>
    public class KnxNetIpRoutingClient : KnxNetIpClient
    {
        public KnxNetIpRoutingClient(KnxNetIpConfiguration configuration = null) : this(new IPEndPoint(IPAddress.Parse("224.0.23.12"), 3671), new KnxDeviceAddress(0,0,0), configuration)
        {
        }

        public KnxNetIpRoutingClient(IPEndPoint remoteEndPoint, KnxDeviceAddress deviceAddress, KnxNetIpConfiguration configuration = null) : base (remoteEndPoint, deviceAddress, configuration)
        {
        }
        
        /// <summary>
        /// Gets a value indicating whether the client is connected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if client is connected; otherwise, <c>false</c>.
        /// </value>
        public override bool IsConnected { get; protected set; }

        /// <summary>
        /// Connects this instance.
        /// </summary>
        public override async Task Connect()
        {
            UdpClient.MulticastLoopback = false;

            await base.Connect();
            
            IsConnected = true;
        }

        protected override void OnKnxNetIpMessageReceived(KnxNetIpMessage message)
        {
            base.OnKnxNetIpMessageReceived(message);

            if (!(message.Body is RoutingIndication routingIndication))
                return;
            
            InvokeKnxMessageReceived(routingIndication.Cemi);
        }

        public override Task Disconnect()
        {
            IsConnected = false;
            return Task.FromResult(true);
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="knxMessage">The KNX message.</param>
        public override async Task SendMessage(IKnxMessage knxMessage)
        {
            // create net ip message
            var netIpMessage = KnxNetIpMessage.Create(KnxNetIpServiceType.RoutingIndication);

            // set knx message
            ((RoutingIndication) netIpMessage.Body).Cemi = knxMessage;

            // send
            Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} SEND => {netIpMessage}");
            
            var bytes = netIpMessage.ToByteArray();
            await UdpClient.SendAsync(bytes, bytes.Length);
        }
    }
}