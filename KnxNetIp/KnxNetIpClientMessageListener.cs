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
        #region Constants and Fields

        /// <summary>
        /// the communication client
        /// </summary>
        private IUdpClient _internalUdpClient;

        /// <summary>
        /// while this is false, the thread will run
        /// </summary>
        private bool _stopThread;

        private AutoResetEvent _stopReceivingHandle = new AutoResetEvent(false);

        private AutoResetEvent _receivingStoppedEvent = new AutoResetEvent(false);

        #endregion

        #region Constructors and Destructors

        #region Constructing / Destructing

        public KnxNetIpClientMessageListener()
        {
            _receivingStoppedEvent = new AutoResetEvent(false);
        }

        public KnxNetIpClientMessageListener(IUdpClient udpClient)
        {
            _internalUdpClient = udpClient;
        }

        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~KnxNetIpClientMessageListener()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_internalUdpClient != null)
                {
                    try
                    {
                        Stop();

                        _receivingStoppedEvent.WaitOne(TimeSpan.FromSeconds(1));
                    }
                    finally
                    {
                        _internalUdpClient = null;
                    }
                }

                if (_stopReceivingHandle != null)
                {
                    _stopReceivingHandle.Dispose();
                    _stopReceivingHandle = null;
                }

                if (_receivingStoppedEvent != null)
                {
                    _receivingStoppedEvent.Dispose();
                    _receivingStoppedEvent = null;
                }
            }
        }

        #endregion

        #region Events

        public event KnxNetIpMessageReceivedHandler KnxNetIpMessageReceived;

        #endregion

        public bool IsListening { get; private set; }

        #region Public Methods

        public void Start()
        {
            _stopThread = false;

            Task.Run(() => ReceiveData());
        }

        public void Stop()
        {
            _stopThread = true;
            
            if (_stopReceivingHandle != null)
            {
                _stopReceivingHandle.Set();
            }
    
            Debug.WriteLine("KNX Listener stopped");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when [KNX message received].
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private void OnKnxMessageReceived(KnxNetIpMessage msg)
        {
            var handler = KnxNetIpMessageReceived;
            if (handler != null)
            {
                handler(this, msg);
            }
        }

        /// <summary>
        /// Receives the data from the UDP client.
        /// </summary>
        private void ReceiveData()
        {
            KnxNetIpMessage lastMessage = null;

            var receivedBuffer = new List<byte>();
            try
            {
                IsListening = true;

                while (!_stopThread)
                {
                    var receivedStuff = _internalUdpClient.Receive(_stopReceivingHandle);
                    if (receivedStuff != null && !_stopThread)
                    {
                        receivedBuffer.AddRange(receivedStuff);

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
            }
            // TODO: Important => think about quitting the thread!
            //catch (ThreadAbortException)
            //{
            //    Debug.WriteLine("KNX Listener Thread aborted.");
            //}
            catch (Exception exception)
            {
                Debug.WriteLine("Listener exception: " + exception.Message);
            }
            finally
            {
                IsListening = false;

                if (_stopReceivingHandle != null)
                {
                    _receivingStoppedEvent.Set();
                }
            }
        }

        #endregion
    }
}