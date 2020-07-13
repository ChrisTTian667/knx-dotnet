using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Knx.Common;
using Knx.DatapointTypes;
using Knx.KnxNetIp;

namespace Knx
{
    // internal class KnxDeviceDriver : KnxDeviceDriverBase, IDisposable
    // {
    //     #region Fields and Constants
    //
    //     private readonly object _connectionStateLock = new object();
    //     private readonly object _syncObject = new object();
    //
    //     #endregion
    //
    //     #region Constructor / Destructor
    //
    //     public KnxDeviceDriver(KnxNetIpConnectionString connectionString)
    //     {
    //         ConnectionString = connectionString;
    //     }
    //
    //     #endregion
    //
    //     #region Properties
    //
    //     public KnxNetIpConnectionString ConnectionString { get; private set; }
    //
    //     public KnxDeviceAddress DeviceAddress
    //     {
    //         get { return ConnectionString == null ? null : ConnectionString.DeviceAddress; }
    //     }
    //
    //     #endregion
    //
    //     #region IDeviceDriver Members
    //
    //     public void Connect()
    //     {
    //         lock (_connectionStateLock)
    //         {
    //             if (IsConnected)
    //             {
    //                 return;
    //             }
    //
    //             var endpoints = new List<ConnectInformation>();
    //
    //             if (NetworkManager.IsLocalNetworkConnected())
    //             {
    //                 if (ConnectionString.InternalProtocol == KnxNetIpProtocol.Tunneling)
    //                 {
    //                     if (!string.IsNullOrWhiteSpace(ConnectionString.InternalAddress))
    //                         endpoints.Add(new ConnectInformation(KnxNetIpProtocol.Tunneling, ConnectionString.InternalAddress, ConnectInformation.ConnectionType.Internal));
    //                 }
    //                 else
    //                 {
    //                     if (!string.IsNullOrWhiteSpace(ConnectionString.InternalMulticastUrl))
    //                         endpoints.Add(new ConnectInformation(KnxNetIpProtocol.Routing, ConnectionString.InternalMulticastUrl, ConnectInformation.ConnectionType.Internal));
    //                 }
    //             }
    //
    //             if (!string.IsNullOrWhiteSpace(ConnectionString.ExternalAddress))
    //                 endpoints.Add(new ConnectInformation(KnxNetIpProtocol.Tunneling, ConnectionString.ExternalAddress, ConnectInformation.ConnectionType.External));
    //
    //             Exception[] errors;
    //             if (!SetConnectionAsync(endpoints.Distinct().ToArray(), out errors))
    //             {
    //                 var message = errors.Aggregate(string.Empty, (current, error) => current + (error.Message + "\n\n"));
    //                 if (errors.Any())
    //                     throw new Exception(message.Trim('\n'), errors.FirstOrDefault());
    //
    //                 throw new Exception("Unable to connect to KNX gateway.");
    //             }
    //         }
    //     }
    //
    //     public void Disconnect()
    //     {
    //         lock (_connectionStateLock)
    //         {
    //             if (Client != null)
    //             {
    //                 try
    //                 {
    //                     Client.Dispose();
    //                 }
    //                 finally
    //                 {
    //                     Client = null;
    //                 }
    //             }
    //         }
    //     }
    //
    //     public void Write(Datatype datatype, Endpoint target)
    //     {
    //         ThrowExceptionIfNotConnected();
    //
    //         var logicalEndpoint = KnxAddress.ParseLogical(target.Address);
    //         Client.Write(logicalEndpoint, datatype.ConvertToDatapointType());
    //     }
    //
    //     public Datatype Read(string dataTypeInfoId, Endpoint source)
    //     {
    //         ThrowExceptionIfNotConnected();
    //
    //         var logicalEndpoint = KnxAddress.ParseLogical(source.Address);
    //
    //         var dpt = Client.Read(DatapointType.GetType(dataTypeInfoId), logicalEndpoint);
    //         var result = new Datatype(MetadataCreator.CreateDatatypeInfo(dpt.GetType()));
    //         result.AssignDatapointTypeValues(dpt);
    //         return result;
    //     }
    //     
    //     #endregion
    //
    //     #region IDisposable Members
    //
    //     public void Dispose()
    //     {
    //         Dispose(true);
    //         GC.SuppressFinalize(this);
    //     }
    //
    //     #endregion
    //
    //     public void Connect(KnxNetIpConnectionString connectionString)
    //     {
    //         Disconnect();
    //         ConnectionString = connectionString;
    //         Connect();
    //     }
    //
    //     protected virtual void Dispose(bool disposing)
    //     {
    //         if ((!disposing) || (Client == null)) return;
    //
    //         Disconnect();
    //     }
    //
    //     private bool SetConnectionAsync(ConnectInformation[] endPoints, out Exception[] errors)
    //     {
    //         Tuple<IKnxClient, ConnectInformation> connectedClient = null;
    //
    //         var threadCount = 0;
    //         var connectErrors = new List<Exception>();
    //         var connectionEvent = new AutoResetEvent(false);
    //
    //         foreach (var info in endPoints)
    //         {
    //             Task.Factory.StartNew((x) =>
    //             {
    //                 var endpoint = (ConnectInformation) x;
    //
    //                 try
    //                 {
    //                     var ipEndpoint = IPEndpointCreator.Create(endpoint.Address);
    //                     var internalClient = endpoint.Protocol == KnxNetIpProtocol.Routing
    //                         ? (IKnxClient) new KnxNetIpRoutingClient(ipEndpoint, DeviceAddress)
    //                         : (IKnxClient) new KnxNetIpTunnelingClient(ipEndpoint, DeviceAddress);
    //
    //                     internalClient.Open();
    //
    //                     lock (_syncObject)
    //                     {
    //                         if (internalClient.IsConnected)
    //                         {
    //                             if (connectedClient == null)
    //                             {
    //                                 connectedClient = new Tuple<IKnxClient, ConnectInformation>(internalClient, endpoint);
    //                                 connectionEvent.Set();
    //                             }
    //                             else
    //                                 internalClient.Dispose();
    //                         }
    //                         else
    //                             internalClient.Dispose();
    //                     }
    //                 }
    //                 catch (Exception ex)
    //                 {
    //                     connectErrors.Add(ex);
    //                     Debug.WriteLine("SetConnectionAsync: " + ex.Message);
    //                 }
    //                 finally
    //                 {
    //                     lock (connectionEvent)
    //                     {
    //                         threadCount++;
    //                     }
    //                 }
    //             }, info);
    //         }
    //
    //         while ((threadCount < endPoints.Count()) && (connectedClient == null))
    //             connectionEvent.WaitOne(TimeSpan.FromMilliseconds(300));
    //
    //         if (connectedClient != null)
    //         {
    //             Client = connectedClient.Item1;
    //             ConnectInfo = connectedClient.Item2;
    //         }
    //         else
    //             Client = null;
    //
    //         errors = connectErrors.ToArray();
    //
    //         return IsConnected;
    //     }
    //
    //     /// <summary>
    //     /// Gets the information about the current connection.
    //     /// </summary>
    //     internal ConnectInformation ConnectInfo { get; private set; }
    //
    //     private void ThrowExceptionIfNotConnected()
    //     {
    //         if (!IsConnected)
    //             throw new InvalidOperationException("Connection to KNX gateway not open.");
    //     }
    //
    //     ~KnxDeviceDriver()
    //     {
    //         Dispose(false);
    //     }
    //
    //     #region Nested type: ConnectInformation
    //
    //     internal class ConnectInformation
    //     {
    //         public enum ConnectionType
    //         {
    //             @Internal,
    //             @External,
    //         }
    //
    //         #region Constructor / Destructor
    //
    //         private ConnectInformation(KnxNetIpProtocol protocol, string address)
    //         {
    //             Protocol = protocol;
    //             Address = address;
    //         }
    //
    //         public ConnectInformation(KnxNetIpProtocol protocol, string address, ConnectionType type) : this(protocol, address)
    //         {
    //             Type = type;
    //         }
    //
    //         #endregion
    //
    //         #region Properties
    //
    //         public KnxNetIpProtocol Protocol { get; private set; }
    //
    //         public string Address { get; private set; }
    //
    //         public ConnectionType Type { get; private set; }
    //
    //         #endregion
    //     }
    //
    //     #endregion
    // }
}