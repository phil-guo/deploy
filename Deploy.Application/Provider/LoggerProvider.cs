using System;
using Deploy.Appliction.Extensions;
using Microsoft.Extensions.Logging;

namespace Deploy.Appliction.Provider
{
    public class LoggerProvider : ILogger
    {
        private readonly string _categoryName;

        public LoggerProvider(string categoryName)
        {
            _categoryName = categoryName;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception).ToString();

            //Utils.TextBoxCallback(message);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NoopDisposable.Instance;
        }
    }

    internal class NoopDisposable : IDisposable
    {
        public static NoopDisposable Instance = new NoopDisposable();

        public void Dispose()
        {
        }
    }
}