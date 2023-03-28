using System.Collections.Concurrent;
using Knx.KnxNetIp;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Knx.Cli.Commands;

public class DiscoverCommand : AsyncCommand<DiscoverCommandSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, DiscoverCommandSettings settings)
    {
        return await AnsiConsole.Status()
            .StartAsync(
                "Discovering KNXnet/IP devices...",
                async ctx =>
                {
                    var devices = new ConcurrentBag<KnxHpai>();
                    await using var client = new KnxNetIpRoutingClient();

                    client.KnxDeviceDiscovered += (_, device) =>
                    {
                        devices.Add(device);
                        ctx.Status($"Discovering KNXnet/IP devices [grey]({devices.Count} found[/])");
                    };

                    await client.ConnectAsync();
                    await client.DiscoverAsync();
                    await Task.Delay(settings.Timeout);

                    // Create a table
                    var table = new Table();
                    table.Border(TableBorder.Rounded);

                    // Add some columns
                    table.AddColumn(new TableColumn("KNXnet/IP Device").LeftAligned());
                    table.AddColumn(new TableColumn("IP-Address").LeftAligned());
                    table.AddColumn(new TableColumn("Device Address").LeftAligned());
                    table.AddColumn(new TableColumn("Mac").LeftAligned());
                    table.AddColumn(new TableColumn("Medium").LeftAligned());

                    foreach (var device in devices)
                    {
                        table.AddRow(
                            new Markup($"[yellow]{device.Description!.FriendlyName}[/]"),
                            new Markup($"[grey]{device.IpAddress}[/]"),
                            new Markup($"[grey]{device.Description!.Address}[/]"),
                            new Markup($"[grey]{BitConverter.ToString(device.Description!.MacAddress)}[/]"),
                            new Markup($"[grey]{device.Description!.Medium}[/]")
                            );
                    }

                    AnsiConsole.Write(table);

                    return 0;
                });
    }

}
