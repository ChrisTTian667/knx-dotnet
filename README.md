# knx-dotnet Library for .NET Core

This is a .NET Core library that allows communication with KNXnet/IP routers and gateways using either the routing or the tunneling protocol. It provides a simple and easy-to-use interface for sending and receiving KNX telegrams over IP as well as device discovery.

## Installation

The library can be installed via NuGet package manager. Simply search for `knx-dotnet` and install the package to your .NET project.

## Usage

#### Sending a message using the routing protocol

```csharp

using Knx.KnxNetIp;

// create a Routing client
using var routingClient = new KnxNetIpRoutingClient();

// connect to the KNXnet/IP gateway
await routingClient.Connect();

// create a fully qualified knx message
var message = new KnxMessage
{
    MessageType = MessageType.Write,
    MessageCode = MessageCode.Request,
    Priority = MessagePriority.Auto,
    SourceAddress = new KnxDeviceAddress(1, 1, 2),
    DestinationAddress = new KnxLogicalAddress(1, 1, 28),
    TransportLayerControlInfo = TransportLayerControlInfo.UnnumberedDataPacket,
    DataPacketCount = 0,
    Payload = new DptBoolean(false).Payload
};

// and send the message
await routingClient.SendMessageAsync(message);

// or use the read or write extensions to keep it as simple as possible
tunnelingClient.Write(KnxAddress.Logical(1,1,28), new DptBoolean(true));

```

#### Sending a message using the tunneling protocol

```csharp

using var tunnelingClient = new KnxNetIpTunnelingClient(
    new IPEndPoint(IPAddress.Parse("10.0.2.5"), 3671),
    KnxAddress.Device(1, 1, 2));

await tunnelingClient.Connect();

tunnelingClient.Write(KnxAddress.Logical(1,1,1), new DptBoolean(true));

```

## Tested with following devices
 - Siemens N148/21 Gateway
 - Weinzierl KNX IP Router 750


If you have tested the library positively or negative with other devices, please let me know.

## License
MIT License

Copyright (c) 2023 Christian Daniel

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.


