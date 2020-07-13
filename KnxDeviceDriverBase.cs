using Knx.Common;
using Knx.KnxNetIp;

namespace Knx
{
    // internal abstract class KnxDeviceDriverBase
    // {
    //     #region Fields and Constants
    //
    //     private IKnxClient _client;
    //
    //     #endregion
    //
    //     #region Properties
    //
    //     protected IKnxClient Client
    //     {
    //         get => _client;
    //         set
    //         {
    //             if (_client != null)
    //                 _client.KnxMessageReceived -= OnKnxMessageReceived;
    //
    //             _client = value;
    //
    //             if (value != null)
    //             {
    //                 _client.KnxMessageReceived -= OnKnxMessageReceived;
    //                 _client.KnxMessageReceived += OnKnxMessageReceived;
    //             }
    //         }
    //     }
    //
    //     public bool IsConnected
    //     {
    //         get
    //         {
    //             var result = (this.Client != null) && this.Client.IsConnected;
    //             return result;
    //         }
    //     }
    //
    //     #endregion
    //
    //     protected virtual void OnKnxMessageReceived(IKnxClient sender, IKnxMessage message)
    //     {
    //         var handler = MessageReceived;
    //         if (handler != null)
    //             handler(sender, message);
    //     }
    //
    //     /// <summary>
    //     /// Occurs when [message received].
    //     /// </summary>
    //     public event MessageReceivedHandler MessageReceived;
    // }
}