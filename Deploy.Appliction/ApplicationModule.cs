using System;
using Autofac;
using Deploy.Appliction.Internal;
using Deploy.Appliction.Internal.Ssh;

namespace Deploy.Appliction
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ChilkatSsh>().As<ISsh>();
        }
    }
}