using Autofac;
using Deploy.Appliction.Internal;
using Deploy.Appliction.Internal.Model;
using Xunit;
using Xunit.Abstractions;

namespace Deploy.Test
{
    public class Sftp_Test : TestBase
    {
        private readonly ISftp _sftp;

        public Sftp_Test(ITestOutputHelper output) : base(output)
        {
            _sftp = Container.ResolveNamed<ISftp>(StrategyDll.SSHNET.ToString());
        }

        [Fact(DisplayName = "同步目录树")]
        public void SyncTreeUpload_Test()
        {
            var localPath = @"D:\test";
            var remotePath = "/home/wwwroot";
            _sftp.SyncTreeUpload(remotePath, localPath);
        }

        [Fact(DisplayName = "创建文件目录")]
        public void CreateDir_Test()
        {
            _sftp.CreateFileDirectory("/home/wwwroot");
        }
    }
}