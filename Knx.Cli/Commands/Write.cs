using System.ComponentModel;
using System.Net;
using Knx.Cli.Configuration;
using Knx.DatapointTypes;
using Knx.DatapointTypes.Dpt1Bit;
using Knx.ExtendedMessageInterface;
using Knx.KnxNetIp;
using Microsoft.Extensions.Options;
using Spectre.Console.Cli;

namespace Knx.Cli.Commands;

public class Write : AsyncCommand<Write.CommandSettings>
{
    private readonly IOptions<KnxOptions> _options;

    public Write(IOptions<KnxOptions> options) =>
        _options = options;

    private IKnxNetIpClient CreateClient() =>
        _options.Value.Protocol == KnxProtocol.Tunneling
            ? new KnxNetIpTunnelingClient(
                options =>
                {
                    options.DeviceAddress = _options.Value.DeviceAddress;
                    options.RemoteAddress = IPAddress.Parse(_options.Value.RemoteAddress);
                })
            : new KnxNetIpRoutingClient(
                options =>
                {
                    options.DeviceAddress = _options.Value.DeviceAddress;
                });

    public override async Task<int> ExecuteAsync(CommandContext context, CommandSettings settings)
    {
        await using var knxNetIpClient = CreateClient();
        await knxNetIpClient.ConnectAsync();

        // TODO: here comes the "funny" part: how to get the DPT from the command line?
        // => we need to interpret based on PropertyAttributes, maybe Json Format or better dynamic parameters with defaults?!
        // => we also need to print a list of available Properties and their potential values for CLI help
        var dpt = DatapointTypeFactory.Create("1.002");

        var message = new KnxMessage
        {
            MessageType = MessageType.Write,
            MessageCode = MessageCode.Request,
            Priority = settings.Priority,
            SourceAddress = _options.Value.DeviceAddress,
            DestinationAddress = (KnxLogicalAddress)settings.DestinationAddress,
            TransportLayerControlInfo = TransportLayerControlInfo.UnnumberedDataPacket,
            DataPacketCount = 0,
            Payload = new DptBoolean(true)
        };

        await knxNetIpClient.SendMessageAsync(message);

        return 0;
    }

    public sealed class CommandSettings : Commands.CommandSettings
    {
        [Description("Message payload")]
        [CommandArgument(1, "<PAYLOAD>")]
        public string Payload { get; init; } = string.Empty;
    }
}
