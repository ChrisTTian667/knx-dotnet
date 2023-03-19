using System.ComponentModel;
using Spectre.Console.Cli;

namespace Knx.Cli.Commands;

public class CommandSettings : Spectre.Console.Cli.CommandSettings
{
    [Description("The destination (Logical) address of the message. E.g. 1/1/1")]
    [CommandArgument(0, "<ADDRESS>")]
    public string DestinationAddress { get; init; } = string.Empty;

    [Description("Show only failed and partial status releases")]
    [CommandOption("-t|--datapointType <DATAPOINT_TYPE>")]
    [DefaultValue("1.001")]
    public string DatapointType { get; init; } = "1.002";

    [Description("The message priority. E.g. System, Normal, Alarm, , Auto")]
    [CommandOption("-p|--priority")]
    [DefaultValue(MessagePriority.Auto)]
    public MessagePriority Priority { get; init; }
}
