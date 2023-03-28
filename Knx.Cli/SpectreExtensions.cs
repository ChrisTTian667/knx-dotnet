using Spectre.Console.Cli;

namespace Knx.Cli;

public static class SpectreExtensions
{
    public static void AddCommand<T>(this IConfigurator<T> configurator, Type commandType, string name)
        where T : CommandSettings =>
        typeof(IConfigurator<T>)
            .GetMethod(nameof(IConfigurator.AddCommand))!
            .MakeGenericMethod(commandType)
            .Invoke(configurator, new object[] { name });
}
