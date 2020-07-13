using Knx.Common;

namespace Knx.KnxNetIp
{
    /// <summary>
    /// Defines the handler for receiving knx messages.
    /// </summary>
    public delegate void MessageReceivedHandler(IKnxClient sender, KnxNetIpMessage netIpMessage);
}