using System;

namespace Knx.KnxNetIp.MessageBody
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    internal class ResponseMessageAttribute : Attribute
    {
        public Type ResponseMessageType { get; private set; }

        public ResponseMessageAttribute(Type responseMessageType)
        {
            ResponseMessageType = responseMessageType;
        }
    }
}
