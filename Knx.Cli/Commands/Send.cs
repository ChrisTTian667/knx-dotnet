using System.ComponentModel;
using Knx.Cli.Configuration;
using Knx.DatapointTypes;
using Knx.DatapointTypes.Dpt1Bit;
using Knx.ExtendedMessageInterface;
using Knx.KnxNetIp;
using Microsoft.Extensions.Options;
using Spectre.Console.Cli;

namespace Knx.Cli.Commands;

public class Send : AsyncCommand<Send.SendSettings>
{
    private readonly IOptions<KnxOptions> _options;

    public Send(IOptions<KnxOptions> options) =>
        _options = options;

    public override async Task<int> ExecuteAsync(CommandContext context, SendSettings sendSettings)
    {
        using var routingClient = new KnxNetIpRoutingClient(
            options =>
            {
                options.DeviceAddress = _options.Value.DeviceAddress;
            });

        await routingClient.ConnectAsync();

        // TODO: here comes the "funny" part: how to get the DPT from the command line?
        // => we need to interpret based on PropertyAttributes, maybe in Json Format?!
        // => we also need to print a list of available Properties and their potential values for CLI help

        var dpt = DatapointTypeFactory.Create("1.002");

        var message = new KnxMessage
        {
            MessageType = MessageType.Write,
            MessageCode = MessageCode.Request,
            Priority = sendSettings.Priority,
            SourceAddress = _options.Value.DeviceAddress,
            DestinationAddress = (KnxLogicalAddress)sendSettings.DestinationAddress,
            TransportLayerControlInfo = TransportLayerControlInfo.UnnumberedDataPacket,
            DataPacketCount = 0,
            Payload = new DptBoolean(true).Payload
        };

        await routingClient.SendMessageAsync(message);

        return 0;
    }

    public class SendSettings : CommandSettings
    {
        [Description("The destination (Logical) address of the message. E.g. 1/1/1")]
        [CommandArgument(0, "<ADDRESS>")]
        public string DestinationAddress { get; init; } = string.Empty;

        [Description("Message payload")]
        [CommandArgument(1, "<PAYLOAD>")]
        public string Payload { get; init; } = string.Empty;

        [Description("Show only failed and partial status releases")]
        [CommandOption("-t|--datapointType <DATAPOINT_TYPE>")]
        [DefaultValue("1.001")]
        public string DatapointType { get; init; } = "1.002";

        [Description("The message priority. E.g. System, Normal, Alarm, , Auto")]
        [CommandOption("-p|--priority")]
        [DefaultValue(MessagePriority.Auto)]
        public MessagePriority Priority { get; init; }
    }
}
