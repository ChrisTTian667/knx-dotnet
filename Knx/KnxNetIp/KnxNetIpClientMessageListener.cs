using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Knx.KnxNetIp
{
    public delegate void KnxNetIpMessageReceivedHandler(object sender, KnxNetIpMessage netIpMessage);

    internal class KnxNetIpClientMessageListener : IDisposable
    {
        private readonly UdpClient _udpClient;
        public event KnxNetIpMessageReceivedHandler KnxNetIpMessageReceived;
        
        public KnxNetIpClientMessageListener(UdpClient udpClient)
        {
            _udpClient = udpClient;

            ReceiveData();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~KnxNetIpClientMessageListener()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
                _udpClient.Dispose();
        }

        /// <summary>
        /// Called when [KNX message received].
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private void OnKnxMessageReceived(KnxNetIpMessage msg)
        {
            KnxNetIpMessageReceived?.Invoke(this, msg);
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
                    var receivedStuff = await _udpClient.ReceiveAsync();
                    var data = receivedStuff.Buffer.ToArray();
                    receivedBuffer.AddRange(data);

                    if (receivedBuffer.Any())
                    {
                        var msg = KnxNetIpMessage.Parse(receivedBuffer.ToArray());
                        receivedBuffer.Clear();

                        try
                        {
                            if (msg != null)
                            {
                                // verify that the message differs from last one.
                                if ((lastMessage != null) && (lastMessage.ServiceType == msg.ServiceType))
                                    if (lastMessage.ToByteArray().SequenceEqual(msg.ToByteArray()))
                                        continue;

                                OnKnxMessageReceived(msg);
                            }
                        }
                        finally
                        {
                            lastMessage = msg;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Listener exception: " + exception.Message);
            }
        }
    }
}