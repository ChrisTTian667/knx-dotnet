using System.Net;
using Knx.Cli.Configuration;
using Knx.DatapointTypes;
using Knx.ExtendedMessageInterface;
using Knx.KnxNetIp;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Knx.Cli.Commands;

public abstract class DatapointTypeCommand<TDatapointType, TSettings> : AsyncCommand<TSettings>
    where TDatapointType : DatapointType
    where TSettings : WriteCommandSettings
{
}

public class WriteDatapointTypeCommand<TDatapointType, TSettings> : DatapointTypeCommand<TDatapointType, TSettings>
    where TDatapointType : DatapointType
    where TSettings : WriteCommandSettings
{
    private readonly IOptions<KnxOptions> _options;

    public WriteDatapointTypeCommand(IOptions<KnxOptions> options)
    {
        _options = options;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, TSettings settings)
    {
        AnsiConsole.WriteLine("Write Command executed! Type: {0}", typeof(TDatapointType).Name);

        try
        {

            await using var knxNetIpClient = CreateClient();
            await knxNetIpClient.ConnectAsync();

            var dpt = DatapointType.Create<TDatapointType>();

            var message = new KnxMessage
            {
                MessageType = MessageType.Write,
                MessageCode = MessageCode.Request,
                Priority = settings.Priority,
                SourceAddress = _options.Value.DeviceAddress,
                DestinationAddress = (KnxLogicalAddress)settings.DestinationAddress,
                TransportLayerControlInfo = TransportLayerControlInfo.UnnumberedDataPacket,
                DataPacketCount = 0,
                Payload = dpt.Payload
            };

            await knxNetIpClient.SendMessageAsync(message);
        }
        catch (Exception e)
        {
            AnsiConsole.WriteLine("Error: {0}", e.Message);
            return 1;
        }

        return 0;
    }

    private IKnxNetIpClient CreateClient()
    {
        return _options.Value.Protocol == KnxProtocol.Tunneling
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
    }
}
