using System;
using Autofac;
using Deploy.Appliction.Internal;
using Deploy.Appliction.Internal.Model;
using Deploy.Appliction.Internal.Sftp;
using Deploy.Appliction.Internal.Ssh;

namespace Deploy.Appliction
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ChilkatSsh>().Named<ISsh>(StrategyDll.Chilkat.ToString());
            builder.RegisterType<ChilkatSftp>().Named<ISftp>(StrategyDll.Chilkat.ToString());

            //builder.RegisterType<SshNetSftp>().Named<ISsh>(StrategyDll.SSHNET.ToString());
            builder.RegisterType<SshNetSftp>().Named<ISftp>(StrategyDll.SSHNET.ToString());
        }
    }
}