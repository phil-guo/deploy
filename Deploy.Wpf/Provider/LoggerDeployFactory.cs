using Microsoft.Extensions.Logging;

namespace Deploy.Wpf.Provider
{
    public class LoggerDeployFactory : ILoggerProvider
    {
        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new LoggerProvider(categoryName);
        }
    }
}