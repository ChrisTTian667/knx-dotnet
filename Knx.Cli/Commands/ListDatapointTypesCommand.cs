using System.Reflection;
using Knx.Common.Attribute;
using Knx.DatapointTypes;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Rendering;

namespace Knx.Cli.Commands;

public class ListDatapointTypesCommand : AsyncCommand<ListCommandSettings>
{
    public override Task<int> ExecuteAsync(CommandContext context, ListCommandSettings settings)
    {

        // Create a table
        var table = new Table();
        table.Border(TableBorder.Rounded);

        // Add some columns
        table.AddColumn("DatapointType");
        table.AddColumn(new TableColumn("Name").Centered());
        table.AddColumn(new TableColumn("Length").LeftAligned());
        table.AddColumn(new TableColumn("Unit").LeftAligned());
        table.AddColumn(new TableColumn("Description").LeftAligned());
        table.AddColumn(new TableColumn("Properties").LeftAligned());

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

            var propertyGrid = new Grid();
            propertyGrid.AddColumn();
            propertyGrid.AddColumn();

            foreach (var property in properties)
            {
                var boolEncoding = property.GetCustomAttributes<BooleanEncodingAttribute>()
                    .FirstOrDefault();

                IRenderable value = new Markup("[grey][/]");

                if (boolEncoding != null)
                {
                    var boolOptions = Markup.Escape($"[{boolEncoding.FalseEncoding}, {boolEncoding.TrueEncoding}]");
                    value = new Markup($"[yellow]{boolOptions}[/]");
                }
                else
                {
                    var unitEncoding = type.GetCustomAttributes<DatapointTypeAttribute>()
                        .FirstOrDefault();
                    if (unitEncoding != null)
                        value = new Markup($"[yellow]{unitEncoding.Unit}[/]");
                }

                propertyGrid.AddRow(new Markup($"[bold]{property.Name}[/]"), value);
            }

            table.AddRow(
                new Markup($"{type.Name}"),
                new Markup($"{datapointType}"),
                new Markup($"{dataLengthAttribute?.Length}"),
                new Markup($"{(datapointType.Unit == Unit.None
                    ? ""
                    : datapointType.Unit)}"),
                new Markup($"{datapointType.Description}"),

                propertyGrid
            );
        }

        AnsiConsole.Write(table);
        return Task.FromResult(0);
    }
}
