using Spectre.Console.Cli;

namespace Knx.Cli;

public static class SpectreExtensions
{
    public static void AddCommand<T>(this IConfigurator<T> configurator, Type commandType, string name)
        where T : CommandSettings
    {
        // if (commandType == null || !typeof(Command).IsAssignableFrom(commandType))
        // {
        //     throw new ArgumentException("The provided type must be a subclass of Spectre.Console.Cli.Command.", nameof(commandType));
        // }

        typeof(IConfigurator<T>)
            .GetMethod(nameof(IConfigurator.AddCommand))!
            .MakeGenericMethod(commandType)
            .Invoke(configurator, new object[] { name });
    }
}
