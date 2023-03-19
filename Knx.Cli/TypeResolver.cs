using Spectre.Console.Cli;

namespace Knx.Cli;

public sealed class TypeResolver : ITypeResolver, IDisposable
{
    private readonly IServiceProvider _serviceProvider;

    public TypeResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Dispose()
    {
        if (_serviceProvider is IDisposable disposable)
            disposable.Dispose();
    }

    public object? Resolve(Type? type)
    {
        return type is null
            ? null
            : _serviceProvider.GetService(type);
    }
}
