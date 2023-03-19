using Spectre.Console.Cli;

namespace Knx.Cli.Commands;

public class Send : AsyncCommand
{
    public override Task<int> ExecuteAsync(CommandContext context)
    {
        return Task.FromResult(-1);
    }
}
