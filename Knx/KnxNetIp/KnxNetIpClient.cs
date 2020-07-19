using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Knx.KnxNetIp
{
    public abstract class KnxNetIpClient : IDisposable
    {
        protected readonly UdpClient UdpClient;

        ~KnxNetIpClient()
        {
            Dispose(false);
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                UdpClient.Dispose();
        }

        /// <summary>
        /// Occurs when [KNX message received].
        /// </summary>
        public event EventHandler<IKnxMessage> KnxMessageReceived;
        
        public IPEndPoint RemoteEndPoint { get; }
        public KnxNetIpConfiguration Configuration { get; }
        public KnxDeviceAddress DeviceAddress { get; }

        public virtual Task Connect()
        {
            UdpClient.Connect(RemoteEndPoint);
            ReceiveData();

            return Task.FromResult(true);
        }
        public abstract bool IsConnected { get; protected set; }
        public abstract Task Disconnect();
        public abstract Task SendMessage(IKnxMessage knxMessage);

        protected KnxNetIpClient(IPEndPoint remoteEndPoint, KnxDeviceAddress deviceAddress, KnxNetIpConfiguration configuration = null)
        {
            Configuration = configuration ?? new KnxNetIpConfiguration();
            RemoteEndPoint = remoteEndPoint ?? throw new ArgumentNullException(nameof(remoteEndPoint));
            DeviceAddress = deviceAddress;

            UdpClient = new UdpClient();
        }

        /// <summary>
        /// Receives the data from the UDP client.
        /// </summary>
        private async void ReceiveData()
        {
            KnxNetIpMessage lastMessage = null;
           
            var receivedBuffer = new List<byte>();
            try
            {
                while (true)
                {
                    var receivedResult = await UdpClient.ReceiveAsync();
                    var receivedData = receivedResult.Buffer.ToArray();
                    receivedBuffer.AddRange(receivedData);
                    if (!receivedBuffer.Any()) 
                        continue;
                    
                    var msg = KnxNetIpMessage.Parse(receivedBuffer.ToArray());
                    if (msg == null) 
                        continue;
                    
                    receivedBuffer.Clear(); 
                        
                    // verify that the message differ from last one.
                    if ((lastMessage != null) && (lastMessage.ServiceType == msg.ServiceType))
                        if (lastMessage.ToByteArray().SequenceEqual(msg.ToByteArray()))
                            continue;

                    OnKnxNetIpMessageReceived(msg);
                    lastMessage = msg;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Listener exception: " + exception.Message);
            }
        }
        
        protected void InvokeKnxMessageReceived(IKnxMessage knxMessage)
        {
            KnxMessageReceived?.Invoke(this, knxMessage);
        }

        protected virtual void OnKnxNetIpMessageReceived(KnxNetIpMessage message)
        {
            if (message != null)
                Debug.WriteLine("{0} RECV <= {1} (HANDLED)", DateTime.Now.ToLongTimeString(), message);
        }
    }
}