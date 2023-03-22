using Knx.DatapointTypes;
using Spectre.Console.Cli;

namespace Knx.Cli.Commands;

public class WriteDatapointType<T> : AsyncCommand<WriteCommandSettings>
    where T : DatapointType
{
    public override Task<int> ExecuteAsync(CommandContext context, WriteCommandSettings settings)
    {
        return Task.FromResult(0);
    }
}
