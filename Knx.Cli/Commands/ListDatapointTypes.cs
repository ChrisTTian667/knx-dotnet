using System.Reflection;
using Knx.Common.Attribute;
using Knx.DatapointTypes;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Knx.Cli.Commands;

public class ListDatapointTypes : AsyncCommand<ListCommandSettings>
{
    public override Task<int> ExecuteAsync(CommandContext context, ListCommandSettings settings)
    {
        foreach (var type in DatapointType.All)
        {
            var dataLengthAttribute = type
                .GetCustomAttributes<DataLengthAttribute>(true)
                .FirstOrDefault();

            var properties = type.GetProperties()
                .Where(p => p.GetCustomAttributes<DatapointPropertyAttribute>(true).Any());

            AnsiConsole.WriteLine("");
            AnsiConsole.WriteLine($"DatapointType: {type.Name}");
            AnsiConsole.WriteLine($" - Length: {dataLengthAttribute?.Length}");
            AnsiConsole.WriteLine($" - {properties.Count()} properties");
            AnsiConsole.WriteLine("Properties:");

            foreach (var property in properties)
            {
                AnsiConsole.WriteLine($" - {property.Name} ({property.PropertyType})");
                // possible values:
            }
        }

        return Task.FromResult(0);
    }
}
