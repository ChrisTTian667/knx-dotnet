using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Knx.Common;
using Knx.Common.Exceptions;
using Knx.DatapointTypes;
using Knx.ExtendedMessageInterface;

namespace Knx
{
    public static class KnxClientExtensions
    {
        /// <summary>
        /// The LearnHandler is invoked each time a KNX-Message is retrieved that matches specified conditions during Learn method of a KNX client. 
        /// </summary>
        /// <returns><c>true</c>, if driver should stop searching, otherwise <c>false</c></returns>
        public delegate bool LearnHandler(KnxAddress destination, object message, DatapointType data);

        public static void Write(this IKnxClient client, KnxLogicalAddress destination, DatapointType data,
                                 MessagePriority priority = MessagePriority.Auto)
        {
            var message = new KnxMessage
                              {
                                  MessageCode = MessageCode.Request,
                                  MessageType = MessageType.Write,
                                  SourceAddress = client.DeviceAddress,
                                  DestinationAddress = destination,
                                  Payload = data.Payload,
                                  Priority = priority,
                              };

            client.SendMessage(message);
        }

        public static void Reply(this IKnxClient client, IKnxMessage replyTo, DatapointType data,
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
                                  Priority = priority,
                              };

            client.SendMessage(message);
        }

        public static T Read<T>(this IKnxClient client, KnxLogicalAddress destination,
                                MessagePriority priority = MessagePriority.Auto, TimeSpan timeOut = default(TimeSpan))
            where T : DatapointType
        {
            return Read(client, typeof(T), destination, priority, timeOut) as T;
        }

        public static DatapointType Read(this IKnxClient client, Type datapointTypeResultType,
                                         KnxLogicalAddress destination, MessagePriority priority = MessagePriority.Auto,
                                         TimeSpan timeOut = default(TimeSpan))
        {
            var replyEvent = new AutoResetEvent(false);
            var indicationPayload = new byte[] { };
            var confirmationPayload = new byte[] { };
            var indicationMessage = default(IKnxMessage);
            var confirmationMessage = default(IKnxMessage);
            

            if (timeOut.TotalMilliseconds <= 0)
                timeOut = client.ReadTimeout;
            if (timeOut.TotalMilliseconds <= 0)
                timeOut = TimeSpan.FromSeconds(5);

            try
            {
                // specify the reply condition
                KnxMessageReceivedHandler onMessageReceived = delegate(IKnxClient sender, IKnxMessage knxMessage)
                {
                    // it not destination -> ignore
                    if (knxMessage.DestinationAddress.ToString() != destination.ToString())
                        return;

                    var indicationCondition = (knxMessage.MessageCode == MessageCode.Indication) && (knxMessage.MessageType == MessageType.Reply);
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
                };

                client.KnxMessageReceived += onMessageReceived;
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
                    client.SendMessage(message);

                    if (replyEvent.WaitOne(timeOut) && indicationMessage != null)
                    {
                        return (DatapointType)Activator.CreateInstance(datapointTypeResultType, indicationPayload);
                    }
                    else
                    {
                        // Fallback, if we retrieved a confirmation, but no indication.
                        if (confirmationMessage != null)
                        {
                            return (DatapointType)Activator.CreateInstance(datapointTypeResultType, confirmationPayload);
                        }

                        throw new KnxTimeoutException(string.Format("Did not retrieve an answer within configured timeout of {0} seconds: {1}", timeOut.TotalSeconds, message));
                    }
                }
                finally
                {
                    client.KnxMessageReceived -= onMessageReceived;
                    Debug.WriteLine("{0} STOPPED READING of {1}", DateTime.Now.ToLongTimeString(), destination);
                }
            }
            finally
            {
                replyEvent.Dispose();
            }
        }

        public static void Learn(this IKnxClient client, Type[] expectedDatapointTypeResultTypes,
                                 LearnHandler stopRequest, MessageType expectedMessageType = MessageType.Write,
                                 TimeSpan duration = default(TimeSpan))
        {
            if (stopRequest == null)
                throw new KnxException("You must specify a LearnHandler method to provide the search results.");

            if (!expectedDatapointTypeResultTypes.Any())
                throw new KnxException("At least one expected Datapoint Type needs to be specified to learn a function.");

            var replyEvent = new AutoResetEvent(false);
            var learnLock = new object();

            try
            {
                KnxMessageReceivedHandler onMessageReceived = delegate(IKnxClient sender, IKnxMessage knxMessage)
                {
                    if ((knxMessage == null) || (knxMessage.MessageType != expectedMessageType)) 
                        return;

                    lock (learnLock)
                    {
                        var payload = new byte[knxMessage.Payload.Length];
                        knxMessage.Payload.CopyTo(payload, 0);

                        foreach (var type in expectedDatapointTypeResultTypes.Distinct())
                        {
                            try
                            {
                                if (!DatapointType.VerifyPayload(type, payload, true))
                                    continue;

                                var dataPointType = Activator.CreateInstance(type, payload) as DatapointType;
                                if (dataPointType != null)
                                {
                                    var handler = stopRequest;
                                    if (handler(knxMessage.DestinationAddress, knxMessage, dataPointType))
                                    {
                                        // we stop here, because application told us to.
                                        replyEvent.Set();
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }
                    }
                };

                try
                {
                    client.KnxMessageReceived += onMessageReceived;
                    replyEvent.WaitOne(duration);
                }
                finally
                {
                    client.KnxMessageReceived -= onMessageReceived;
                }
            }
            finally
            {
                replyEvent.Dispose();
            }
        }
    }
}