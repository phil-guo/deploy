using Microsoft.Extensions.Logging;

namespace Deploy.Appliction.Provider
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