using System;
using System.Diagnostics;
using System.Threading;
using Knx.DatapointTypes;
using Knx.ExtendedMessageInterface;
using Knx.KnxNetIp;
using TimeoutException = Knx.Exceptions.TimeoutException;

namespace Knx;

public static class KnxClientExtensions
{
    public static void Write(
        this IKnxNetIpClient client,
        KnxLogicalAddress destination,
        DatapointType data,
        MessagePriority priority = MessagePriority.Auto)
    {
        var message = new KnxMessage
        {
            MessageCode = MessageCode.Request,
            MessageType = MessageType.Write,
            DestinationAddress = destination,
            Payload = data.Payload,
            Priority = priority
        };

        client.SendMessageAsync(message);
    }

    public static void Reply(
        this IKnxNetIpClient client,
        IKnxMessage replyTo,
        DatapointType data,
        MessagePriority priority = MessagePriority.Auto)
    {
        var message = new KnxMessage
        {
            MessageCode = MessageCode.Confirmation,
            MessageType = replyTo.MessageType,
            DestinationAddress = replyTo.SourceAddress,
            Payload = data.Payload,
            Priority = priority
        };

        client.SendMessageAsync(message);
    }

    public static T Read<T>(
        this IKnxNetIpClient client,
        KnxLogicalAddress destination,
        MessagePriority priority = MessagePriority.Auto,
        TimeSpan timeOut = default) where T : DatapointType
    {
        return Read(client, typeof(T), destination, priority, timeOut) as T;
    }

    public static DatapointType Read(
        this IKnxNetIpClient client,
        Type datapointTypeResultType,
        KnxLogicalAddress destination,
        MessagePriority priority = MessagePriority.Auto,
        TimeSpan timeOut = default)
    {
        var replyEvent = new AutoResetEvent(false);
        var indicationPayload = Array.Empty<byte>();
        var confirmationPayload = Array.Empty<byte>();
        var indicationMessage = default(IKnxMessage);
        var confirmationMessage = default(IKnxMessage);

        if (timeOut.TotalMilliseconds <= 0)
            timeOut = TimeSpan.FromSeconds(10);

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

                    throw new TimeoutException(
                        $"Did not retrieve an answer within configured timeout of {timeOut.TotalSeconds} seconds: {message}");
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
