using System;
using System.Collections.Generic;
using Deploy.Appliction.Config;
using Deploy.Appliction.Extensions;
using Microsoft.Extensions.Logging;
using Renci.SshNet;

namespace Deploy.Appliction.Internal.Ssh
{
    public class SshNet : ISsh
    {
        private readonly Dictionary<string, SshClient> _dictionary = new Dictionary<string, SshClient>();

        private const string Name = "SshClient";

        private readonly ILogger<SshNet> _logger;

        public SshNet(ILogger<SshNet> logger)
        {
            _logger = logger;
        }

        private SshClient CreateSshClient()
        {
            if (_dictionary.TryGetValue(Name, out var ssh))
                return ssh;
            var config = AppConfig.Default.Deploy;
            var connectionInfo = new ConnectionInfo(config.Host,
                config.Port,
                config.Root,
                new PasswordAuthenticationMethod(config.Root, config.Password));
            var client = new SshClient(connectionInfo);
            _dictionary.TryAdd(Name, client);
            return client;
        }

        public void SendCommands(string cmd)
        {
            Utils.TryCatchAction(() =>
            {
                using var client = CreateSshClient();

                client.Connect();

                _logger.LogInformation("【创建ssh链接成功】....");

                var result = client.RunCommand(cmd);
                if (!string.IsNullOrEmpty(result.Error))
                    _logger.LogWarning($"$Warning【执行 {cmd} 命令 ... 返回结果：{result.Error}】");
                else if (cmd.Contains("logs"))
                {
                    _logger.LogInformation($"                                   ----------------【容器启动日志】-------------");
                    _logger.LogInformation(result.Result);
                }
                else
                {
                    _logger.LogInformation($"$执行 {cmd} 命令成功 ...");
                }

                _dictionary.Remove(Name);
            });
        }

        public void SendCommand(SshClient ssh, string cmd)
        {
            var result = ssh.RunCommand(cmd);
            if (!string.IsNullOrEmpty(result.Error))
                _logger.LogWarning($"$Warning【执行 {cmd} 命令 ... 返回结果：{result.Error}】");
            else if (cmd.Contains("logs"))
            {
                _logger.LogInformation($"                                   ----------------【容器启动日志】-------------");
                _logger.LogInformation(result.Result);
            }
            else
            {
                _logger.LogInformation($"$执行 {cmd} 命令成功 ...");
            }
        }

        public void ExecuteFrontCmd(string dockerName, string imageName)
        {
            Utils.TryCatchAction(() =>
            {
                var appConfig = AppConfig.Default.Deploy;

                using var client = CreateSshClient();

                client.Connect();

                _logger.LogInformation("【创建ssh链接成功】....");


                SendCommand(client, $"docker stop {dockerName}");
                SendCommand(client, $"docker rm {dockerName}");
                SendCommand(client, $"docker rmi {imageName}");
                SendCommand(client, $"cd {appConfig.RemotePath}; docker build -t {imageName} .");
                var runCmd =
                    $"docker run --name={dockerName} -itd -p {appConfig.MapperPort} --restart=always {imageName}";

                SendCommand(client, runCmd);

                SendCommand(client, $"docker logs {dockerName}");

                _dictionary.Remove(Name);
            });
        }
    }
}