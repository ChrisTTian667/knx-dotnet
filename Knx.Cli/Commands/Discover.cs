using Knx.KnxNetIp;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Knx.Cli.Commands;

public class Discover : AsyncCommand
{
    public override async Task<int> ExecuteAsync(CommandContext context) =>
        await AnsiConsole.Status()
            .StartAsync(
                "Discover KnxNetIp devices in your network.",
                async _ =>
                {
                    var routingClient = new KnxNetIpRoutingClient(
                        options =>
                        {
                            options.DeviceAddress = "1/1/2";
                        });

                    routingClient.KnxDeviceDiscovered += (_, args) =>
                    {
                        AnsiConsole.MarkupLine($"[green]Discovered device:[/] {args.FriendlyName}");
                    };

                    await routingClient.ConnectAsync();
                    await routingClient.DiscoverAsync();

                    await Task.Delay(60000);

                    return 0;
                });
}
