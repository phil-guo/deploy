using System.Collections.Concurrent;
using Chilkat;
using Deploy.Appliction.Config;
using Microsoft.Extensions.Logging;

namespace Deploy.Appliction.Internal
{
    public class ChilkatSsh
    {
        public static readonly ConcurrentDictionary<string, Ssh> SshDictionary = new ConcurrentDictionary<string, Ssh>();
        private readonly ILogger<ChilkatSsh> _logger;

        protected ChilkatSsh(ILogger<ChilkatSsh> logger)
        {
            _logger = logger;
            SshDictionary.TryAdd("ssh", CreateSshClient());
        }

        protected Ssh CreateSshClient()
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
    }
}