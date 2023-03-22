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

public class List : AsyncCommand<List.CommandSettings>
{
    private readonly IOptions<KnxOptions> _options;

    public List(IOptions<KnxOptions> options) =>
        _options = options;

    public override Task<int> ExecuteAsync(CommandContext context, CommandSettings settings)
    {
        switch (settings.Type)
        {
            case ListType.DatapointTypes:
                DatapointTypeFactory.GetAll();
                break;
        }

        return Task.FromResult(0);
    }

    public enum ListType
    {
        [Description("List all datapoint types")]
        DatapointTypes,
    }

    public sealed class CommandSettings : Spectre.Console.Cli.CommandSettings
    {
        [Description("Objecttype to list")]

        [CommandArgument(1, "<TYPE>")]
        public ListType Type { get; init; }
    }
}
