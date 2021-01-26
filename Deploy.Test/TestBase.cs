using System;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Deploy.Appliction;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Deploy.Test
{
    public class TestBase
    {
        private readonly ITestOutputHelper _output;

        protected ILifetimeScope Container { get; set; }

        public TestBase(ITestOutputHelper output)
        {
            _output = output;
            Init();
        }

        private void Init()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            var container = new ContainerBuilder();
            container.Populate(serviceCollection);
            BuildUpContainer(container);
            Container = container.Build();
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