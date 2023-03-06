using System;
using System.Diagnostics;
using System.Threading;
using Knx.DatapointTypes;
using Knx.Exceptions;
using Knx.ExtendedMessageInterface;
using Knx.KnxNetIp;

namespace Knx;

public static class KnxClientExtensions
{
    public static void Write(
        this KnxNetIpClient client,
        KnxLogicalAddress destination,
        DatapointType data,
        MessagePriority priority = MessagePriority.Auto)
    {
        var message = new KnxMessage
        {
            MessageCode = MessageCode.Request,
            MessageType = MessageType.Write,
            SourceAddress = client.DeviceAddress,
            DestinationAddress = destination,
            Payload = data.Payload,
            Priority = priority
        };

        client.SendMessageAsync(message);
    }

    public static void Reply(
        this KnxNetIpClient client,
        IKnxMessage replyTo,
        DatapointType data,
        MessagePriority priority = MessagePriority.Auto)
    {
        var request = replyTo;
        var message = new KnxMessage
        {
            MessageCode = MessageCode.Confirmation,
            MessageType = replyTo.MessageType,
            SourceAddress = client.DeviceAddress,
            DestinationAddress = request.SourceAddress,
            Payload = data.Payload,
            Priority = priority
        };

        client.SendMessageAsync(message);
    }

    public static T Read<T>(
        this KnxNetIpClient client,
        KnxLogicalAddress destination,
        MessagePriority priority = MessagePriority.Auto,
        TimeSpan timeOut = default) where T : DatapointType
    {
        return Read(client, typeof(T), destination, priority, timeOut) as T;
    }

    public static DatapointType Read(
        this KnxNetIpClient client,
        Type datapointTypeResultType,
        KnxLogicalAddress destination,
        MessagePriority priority = MessagePriority.Auto,
        TimeSpan timeOut = default)
    {
        var replyEvent = new AutoResetEvent(false);
        var indicationPayload = new byte[] { };
        var confirmationPayload = new byte[] { };
        var indicationMessage = default(IKnxMessage);
        var confirmationMessage = default(IKnxMessage);

        if (timeOut.TotalMilliseconds <= 0)
            timeOut = client.Configuration.ReadTimeout;

        try
        {
            // specify the reply condition
            void KnxMessageReceived(object sender, IKnxMessage knxMessage)
            {
                if (knxMessage.DestinationAddress.ToString() != destination.ToString())
                    return;

                var indicationCondition = knxMessage.MessageCode == MessageCode.Indication &&
                                          knxMessage.MessageType == MessageType.Reply;
                var confirmationCondition = knxMessage.MessageCode == MessageCode.Confirmation;


                // If we receive a confirmation, we cannot be sure that the payload correspond to the current actor value
                if (confirmationCondition)
                {
                    confirmationMessage = knxMessage;
                    confirmationPayload = new byte[knxMessage.PayloadLength];
                    knxMessage.Payload.CopyTo(confirmationPayload, 0);
                }

                // but, if we receive an indication, we can be sure, that the payload is the current actor value
                if (indicationCondition)
                {
                    indicationMessage = knxMessage;
                    indicationPayload = new byte[knxMessage.PayloadLength];
                    knxMessage.Payload.CopyTo(indicationPayload, 0);
                    replyEvent.Set();
                }
            }

            client.KnxMessageReceived += KnxMessageReceived;
            try
            {
                // create and send the read request
                var message = new KnxMessage
                {
                    MessageCode = MessageCode.Request,
                    MessageType = MessageType.Read,
                    SourceAddress = client.DeviceAddress,
                    DestinationAddress = destination,
                    Priority = priority
                };

                Debug.WriteLine("{0} START READING from {1}", DateTime.Now.ToLongTimeString(), destination);
                client.SendMessageAsync(message);

                if (replyEvent.WaitOne(timeOut) && indicationMessage != null)
                    return (DatapointType)Activator.CreateInstance(datapointTypeResultType, indicationPayload);
                else
                {
                    // Fallback, if we retrieved a confirmation, but no indication.
                    if (confirmationMessage != null)
                        return (DatapointType)Activator.CreateInstance(datapointTypeResultType, confirmationPayload);

                    throw new KnxTimeoutException(
                        string.Format(
                            "Did not retrieve an answer within configured timeout of {0} seconds: {1}",
                            timeOut.TotalSeconds,
                            message));
                }
            }
            finally
            {
                client.KnxMessageReceived -= KnxMessageReceived;
                Debug.WriteLine("{0} STOPPED READING of {1}", DateTime.Now.ToLongTimeString(), destination);
            }
        }
        finally
        {
            replyEvent.Dispose();
        }
    }
}
