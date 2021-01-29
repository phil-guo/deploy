using Autofac;
using Deploy.Appliction.Internal;
using Xunit;
using Xunit.Abstractions;

namespace Deploy.Test
{
    public class Ssh_Test : TestBase
    {
        private readonly ISsh _ssh;

        public Ssh_Test(ITestOutputHelper output) : base(output)
        {
            _ssh = Container.Resolve<ISsh>();
        }

        [Fact(DisplayName = "发送shell命令")]
        public void SendCommands_Test()
        {
            // _ssh.SendCommands("docker stop gr");
            // _ssh.SendCommands("docker rm gr");
            // _ssh.SendCommands("docker run -itd --name=gr -p 9001:80 gl");
            
            _ssh.SendCommands("cd /root/wwwroot");
            _ssh.SendCommands("docker build -t test .");
        }
    }
}