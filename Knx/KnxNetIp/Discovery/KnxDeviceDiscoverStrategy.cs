// using System;
// using System.Threading;
// using System.Threading.Tasks;
// using Knx.KnxNetIp.MessageBody;
// using PortableDI;
//
// namespace Knx.KnxNetIp.Discovery
// {
//     public class KnxDeviceDiscoverStrategy : IDisposable
//     {
//         #region Static Fields and Constants
//
//         private static readonly KnxDeviceDiscoverStrategy Instance = new KnxDeviceDiscoverStrategy();
//
//         #endregion
//
//         #region Fields and Constants
//
//         private readonly object _udpClientLock = new object();
//
//         private IMulticastUdpClient _udpClient;
//
//         #endregion
//
//         #region Constructor / Destructor
//
//         /// <summary>
//         ///     Prevents a default instance of the <see cref="KnxDeviceDiscoverStrategy" /> class from being created.
//         /// </summary>
//         public KnxDeviceDiscoverStrategy()
//         {
//         }
//
//         #endregion
//
//         #region Properties
//
//         /// <summary>
//         ///     Gets the current Gateway discoverer
//         /// </summary>
//         public static KnxDeviceDiscoverStrategy Current
//         {
//             get { return Instance; }
//         }
//
//         #endregion
//
//         #region IDeviceDiscoverStrategy Members
//
//         /// <summary>
//         ///     Sends an asynchronous Discover request
//         /// </summary>
//         public void StartAsync()
//         {
//             Task.Run(() => SendDiscoverRequest());
//         }
//
//         public void Stop()
//         {
//             DisposeUpdClient();
//         }
//
//         public event EventHandler<DeviceDiscoveredEventArgs> DeviceDiscovered;
//
//         #endregion
//
//         #region IDisposable Members
//
//         public void Dispose()
//         {
//             Dispose(true);
//             GC.SuppressFinalize(this);
//         }
//
//         #endregion
//
//         private DeviceInfo CreateDeviceInfoFromKnxHpai(KnxHpai endpoint)
//         {
//             var connection = new KnxNetIpConnectionString()
//             {
//                 InternalAddress = endpoint.IpAddress + ":" + endpoint.Port,
//                 DeviceMain = endpoint.Description.Address.Area,
//                 DeviceMiddle = endpoint.Description.Address.Line,
//                 DeviceSub = endpoint.Description.Address.Device
//             };
//             return new DeviceInfo("KnxDevice", connection.ToString());
//         }
//
//         private void Dispose(bool disposing)
//         {
//             if (disposing)
//             {
//                 DisposeUpdClient();
//             }
//         }
//
//         private void DisposeUpdClient()
//         {
//             lock (_udpClientLock)
//             {
//                 if (_udpClient != null)
//                 {
//                     try
//                     {
//                         _udpClient.Received -= OnDataReceived;
//                         _udpClient.Dispose();
//                         _udpClient = null;
//                     }
//                     catch
//                     {
//                     }
//                 }
//             }
//         }
//
//         private void InitializeUdpClient()
//         {
//             if (_udpClient != null && _udpClient.LocalEndpoint != null)
//             {
//                 return;
//             }
//
//             // Open UDP Connection to KNX-NetIp Multicast
//             _udpClient = DIContainer.Resolve<IMulticastUdpClient>();
//             try
//             {
//                 _udpClient.Connect(Defaults.IPv4MulticastAddress);
//                 _udpClient.Received += OnDataReceived;
//             }
//             catch (Exception)
//             {
//                 _udpClient.Dispose();
//                 _udpClient = null;
//             }
//         }
//
//         private void OnDataReceived(object sender, BytesReceivedEventArgs eventArgs)
//         {
//             KnxNetIpMessage<SearchResponse> response;
//             var e = eventArgs.Bytes;
//
//             if (!KnxNetIpMessage.TryParse(e, out response))
//                 return;
//
//             OnDeviceDiscovered(CreateDeviceInfoFromKnxHpai(response.Body.Endpoint), response.Body.Endpoint.Description.FriendlyName);
//         }
//
//         private void OnDeviceDiscovered(DeviceInfo deviceInfo, string friendlyName)
//         {
//             var handler = DeviceDiscovered;
//             if (handler != null)
//                 handler(this, new DeviceDiscoveredEventArgs(deviceInfo, friendlyName));
//         }
//
//         private void SendDiscoverRequest()
//         {
//             // try another two times, if it not works for the first time.
//             for (var discoverTry = 0; discoverTry < 2; discoverTry++)
//             {
//                 try
//                 {
//                     lock (_udpClientLock)
//                     {
//                         InitializeUdpClient();
//
//                         if ((_udpClient == null) || (_udpClient.LocalEndpoint == null))
//                             continue;
//
//                         var localRequest = MessageFactory.GetSearchRequest(_udpClient.LocalEndpoint);
//                         var remoteRequest = MessageFactory.GetSearchRequest(_udpClient.RemoteEndpoint);
//
//                         _udpClient.Send(localRequest.ToByteArray());
//                         _udpClient.Send(remoteRequest.ToByteArray());
//                     }
//
//                     return;
//                 }
//                 catch (Exception ex)
//                 {
//                     new AutoResetEvent(false).WaitOne(3000);
//                 }
//             }
//         }
//
//         ~KnxDeviceDiscoverStrategy()
//         {
//             Dispose(false);
//         }
//     }
// }