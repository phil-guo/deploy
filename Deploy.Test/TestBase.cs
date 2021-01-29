using System;
using System.IO;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Deploy.Appliction;
using Deploy.Appliction.Provider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Deploy.Test
{
    public abstract class TestBase
    {
        private readonly ITestOutputHelper _output;

        protected ILifetimeScope Container { get; set; }

        protected TestBase(ITestOutputHelper output)
        {
            _output = output;
            Init();
        }

        private void Init()
        {
            var builder = new ContainerBuilder();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? string.Empty, "Config"))
                .AddJsonFile("appsettings.json", optional: true);
            var configuration = configurationBuilder.Build();
            serviceCollection.AddSingleton<IConfiguration>(configuration);

            builder.Populate(serviceCollection);

            BuildUpContainer(builder);
            Appliction.Extensions.Utils.Current = builder.Build();

            Container = Appliction.Extensions.Utils.Current;

            var loggerFactory = Appliction.Extensions.Utils.Current.Resolve<ILoggerFactory>();
            loggerFactory.AddProvider(new LoggerDeployFactory());
        }

        private void BuildUpContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<ApplicationModule>();
        }

        protected void Print(object obj)
        {
            _output.WriteLine(JsonConvert.SerializeObject(obj));
        }
    }
}