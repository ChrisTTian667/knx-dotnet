namespace Knx.Cli.Commands;

internal static class DatapointTypeCommandBuilder
{
    public static Type CreateDatapointTypeCommand(Type datapointType)
    {
        var builder = new DatapointTypeSettingsBuilder(datapointType);
        var commandSettingsType = builder.CreateSettings();

        var commandType = typeof(WriteDatapointTypeCommand<,>)
            .MakeGenericType(datapointType, commandSettingsType);

        return commandType;
    }

    public static Type CreateCommand(Type commandBaseType, Type dataType)
    {
        var builder = new DatapointTypeSettingsBuilder(dataType);
        var commandSettingsType = builder.CreateSettings();

        var commandType = commandBaseType
            .MakeGenericType(dataType, commandSettingsType);

        return commandType;
    }
}
