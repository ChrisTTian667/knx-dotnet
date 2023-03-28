using System.ComponentModel;
using Spectre.Console.Cli;

namespace Knx.Cli.Commands;

public class DiscoverCommandSettings : CommandSettings
{
    [Description("The timeout in milliseconds for the discovery process.")]
    [CommandOption("-t|--timeout")]
    [DefaultValue(1500)]
    public int Timeout { get; init; }
}
