namespace Knx.KnxNetIp;

/// <summary>
///     Defines the handler for receiving knx messages.
/// </summary>
public delegate void MessageReceivedHandler(KnxNetIpClient sender, KnxNetIpMessage netIpMessage);