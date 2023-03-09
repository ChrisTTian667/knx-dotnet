using System;

namespace Knx.KnxNetIp.MessageBody;

[AttributeUsage(AttributeTargets.Class)]
internal class ResponseMessageAttribute : Attribute
{
    public ResponseMessageAttribute(Type responseMessageType)
    {
        ResponseMessageType = responseMessageType;
    }

    public Type ResponseMessageType { get; }
}