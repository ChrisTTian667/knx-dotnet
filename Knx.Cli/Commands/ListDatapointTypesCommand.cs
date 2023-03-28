using System.Reflection;
using Knx.Common.Attribute;
using Knx.DatapointTypes;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Knx.Cli.Commands;

public class ListDatapointTypesCommand : AsyncCommand<ListCommandSettings>
{
    public override Task<int> ExecuteAsync(CommandContext context, ListCommandSettings settings)
    {

        // Create a table
        var table = new Table();

        // Add some columns
        table.AddColumn("DatapointType");
        table.AddColumn(new TableColumn("Name").Centered());
        table.AddColumn(new TableColumn("Length").LeftAligned());
        table.AddColumn(new TableColumn("Unit").LeftAligned());
        table.AddColumn(new TableColumn("Description").LeftAligned());

        // Add some rows
        // table.AddRow("Baz", "[green]Qux[/]");
        // table.AddRow(new Markup("[blue]Corgi[/]"), new Panel("Waldo"));

        foreach (var type in DatapointType.All)
        {
            var dataLengthAttribute = type
                .GetCustomAttributes<DataLengthAttribute>(true)
                .First();
            var datapointType = type
                .GetCustomAttributes<DatapointTypeAttribute>(true)
                .First();
            var properties = type.GetProperties()
                .Where(p => p.GetCustomAttributes<DatapointPropertyAttribute>(true).Any());

            table.AddRow(
                new Markup($"{type.Name}"),
                new Markup($"{datapointType}"),
                new Markup($"{dataLengthAttribute?.Length}"),
                new Markup($"{(datapointType.Unit == Unit.None
                    ? ""
                    : datapointType.Unit)}"),
                new Markup($"{datapointType.Description}"));

            AnsiConsole.WriteLine("");
            AnsiConsole.WriteLine($"DatapointType: {type.Name}");
            AnsiConsole.WriteLine($" - Length: {dataLengthAttribute?.Length}");
            AnsiConsole.WriteLine($" - {properties.Count()} properties");
            AnsiConsole.WriteLine("Properties:");

            // foreach (var property in properties)
            // {
            //     AnsiConsole.WriteLine($" - {property.Name} ({property.PropertyType})");
            //     // possible values:
            //
            //     var boolEncoding = property.GetCustomAttribute<BooleanEncodingAttribute>();
            //     if (boolEncoding != null)
            //     {
            //         AnsiConsole.WriteLine($"   > {boolEncoding.FalseEncoding}");
            //         AnsiConsole.WriteLine($"   > {boolEncoding.TrueEncoding}");
            //     }
            //     else
            //     {
            //         var unitEncoding = type.GetCustomAttribute<DatapointTypeAttribute>();
            //         if (unitEncoding != null) AnsiConsole.WriteLine($"   > {unitEncoding.Unit}");
            //     }
            // }
        }

        AnsiConsole.Write(table);

        return Task.FromResult(0);
    }
}
