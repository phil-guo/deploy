using System.Collections.Concurrent;
using System.Threading;
using Deploy.Appliction.Config;
using Microsoft.Extensions.Logging;

namespace Deploy.Appliction.Internal.Ssh
{
    public class ChilkatSsh : ISsh
    {
        private static readonly ConcurrentDictionary<string, Chilkat.Ssh> SshDictionary =
            new ConcurrentDictionary<string, Chilkat.Ssh>();

        private const string Name = "ssh";

        private readonly ILogger<ChilkatSsh> _logger;

        public ChilkatSsh(ILogger<ChilkatSsh> logger)
        {
            _logger = logger;
            if (!SshDictionary.TryGetValue(Name, out var ssh))
            {
                ssh = CreateSshClient();
                SshDictionary.TryAdd(Name, ssh);
            }

            if (ssh != null)
                _logger.LogInformation("ssh 链接创建成功");
            else
            {
                SshDictionary.TryRemove(Name, out ssh);
                SshDictionary.TryAdd(Name, CreateSshClient());
            }
        }

        private Chilkat.Ssh CreateSshClient()
        {
            var ssh = new Chilkat.Ssh()
            {
                IdleTimeoutMs = AppConfig.Default.Deploy.TimeOutMs
            };
            var success = ssh.Connect(AppConfig.Default.Deploy.Host, AppConfig.Default.Deploy.Port);

            if (!success)
            {
                _logger.LogInformation($"链接服务器失败，请检查host 与 端口 ----- {ssh.LastErrorText}");
                return null;
            }

            success = ssh.AuthenticatePw(AppConfig.Default.Deploy.Root, AppConfig.Default.Deploy.Password);

            if (!success)
            {
                _logger.LogInformation($"链接服务器登录失败，请检查登录名与密码 ----- {ssh.LastErrorText}");
                return null;
            }

            return ssh;
        }

        public void ExecuteFrontCmd(string dockerName, string imageName)
        {
            var appConfig = AppConfig.Default.Deploy;

            SendCommands($"docker stop {dockerName}");
            SendCommands($"docker rm {dockerName}");
            SendCommands($"docker rmi {imageName}");
            SendCommands($"cd {appConfig.RemotePath}; docker build -t {imageName} .");
            var runCmd =
                $"docker run --name={dockerName} -itd -p {appConfig.MapperPort} --restart=always {imageName}";

            SendCommands(runCmd);
        }

        public void SendCommands(string cmd)
        {
            SshDictionary.TryGetValue(Name, out var ssh);
            if (ssh == null)
                _logger.LogInformation("没有ssh上下文 请先创建ssh上下文");

            var channelNum = ssh.OpenSessionChannel();
            if (channelNum < 0)
                _logger.LogInformation(ssh.LastErrorText);

            var success = ssh.SendReqExec(channelNum, cmd);

            if (!success)
            {
                _logger.LogInformation(ssh.LastErrorText);
                return;
            }

            success = ssh.ChannelReceiveToClose(channelNum);
            if (!success)
            {
                _logger.LogInformation(ssh.LastErrorText);
                return;
            }

            _logger.LogInformation($"{cmd}  ---命令执行成功--");
        }


        public void Dispose()
        {
            SshDictionary[Name].Disconnect();
        }
    }
}