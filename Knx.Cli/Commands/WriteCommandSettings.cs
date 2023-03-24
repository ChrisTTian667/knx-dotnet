using System.ComponentModel;
using Spectre.Console.Cli;

namespace Knx.Cli.Commands;

public class WriteCommandSettings : CommandSettings
{
    [Description("The destination (Logical) address of the message. E.g. 1/1/1")]
    [CommandArgument(0, "<ADDRESS>")]
    public string DestinationAddress { get; init; } = string.Empty;

    [Description("The message priority. e.g. System, Normal, Alarm, Auto")]
    [CommandOption("-p|--priority")]
    [DefaultValue(MessagePriority.Auto)]
    public MessagePriority Priority { get; init; }
}
