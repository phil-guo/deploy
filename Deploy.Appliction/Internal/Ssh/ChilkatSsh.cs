using System.Collections.Concurrent;
using Deploy.Appliction.Config;
using Microsoft.Extensions.Logging;

namespace Deploy.Appliction.Internal.Ssh
{
    public class ChilkatSsh : ISsh
    {
        private static readonly ConcurrentDictionary<string, Chilkat.Ssh> SshDictionary =
            new ConcurrentDictionary<string, Chilkat.Ssh>();

        private readonly ILogger<ChilkatSsh> _logger;

        public ChilkatSsh(ILogger<ChilkatSsh> logger)
        {
            _logger = logger;
            SshDictionary.TryAdd("ssh", CreateSshClient());
        }

        private Chilkat.Ssh CreateSshClient()
        {
            var ssh = new Chilkat.Ssh()
            {
                IdleTimeoutMs = AppConfig.Default.Deploy.TimeOutMs
            };
            var success = ssh.Connect(AppConfig.Default.Deploy.Host, AppConfig.Default.Deploy.Port);

            if (!success)
                _logger.LogInformation($"链接服务器失败，请检查host 与 端口 ----- {ssh.LastErrorText}");

            success = ssh.AuthenticatePw(AppConfig.Default.Deploy.Root, AppConfig.Default.Deploy.Password);

            if (!success)
                _logger.LogInformation($"链接服务器登录失败，请检查登录名与密码 ----- {ssh.LastErrorText}");

            if (!success)
                return null;

            return ssh;
        }

        public void SendCommands(string cmd)
        {
            SshDictionary.TryGetValue("ssh", out var ssh);
            if (ssh == null)
                _logger.LogInformation("没有ssh上下文 请先创建ssh上下文");

            var channelNum = ssh.OpenSessionChannel();
            if (channelNum < 0)
                _logger.LogInformation(ssh.LastErrorText);

            var success = ssh.SendReqExec(channelNum, cmd);

            if (!success)
                _logger.LogInformation(ssh.LastErrorText);

            success = ssh.ChannelReceiveToClose(channelNum);
            if (!success)
                _logger.LogInformation(ssh.LastErrorText);

            // var cmdOutput = ssh.GetReceivedText(channelNum, cmd);
            // if (!ssh.LastMethodSuccess)
            //     _logger.LogInformation(ssh.LastErrorText);
            //
            // _logger.LogInformation(cmdOutput);
        }


        public void Dispose()
        {
            SshDictionary["ssh"].Disconnect();
        }
    }
}