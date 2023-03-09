namespace Knx.KnxNetIp;

/// <summary>
///     Defines the handler for receiving knx messages.
/// </summary>
public delegate void MessageReceivedHandler(IKnxNetIpClient sender, KnxNetIpMessage netIpMessage);
