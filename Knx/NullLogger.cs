using System;
using Microsoft.Extensions.Logging;

namespace Knx;

internal class NullLogger<T> : ILogger<T>
{
    public static readonly NullLogger<T> Instance = new();

    public IDisposable BeginScope<TState>(TState state)
    {
        return NullScope.Instance;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return false;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception, string> formatter)
    {
        // Do nothing
    }

    private class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();

        private NullScope()
        {
        }

        public void Dispose()
        {
            // Do nothing
        }
    }
}