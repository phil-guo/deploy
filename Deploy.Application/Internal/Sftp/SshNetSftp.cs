using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Deploy.Appliction.Config;
using Deploy.Appliction.Extensions;
using Microsoft.Extensions.Logging;
using Renci.SshNet;

namespace Deploy.Appliction.Internal.Sftp
{
    public class SshNetSftp : ISftp
    {
        private readonly Dictionary<string, SftpClient> _dictionary = new Dictionary<string, SftpClient>();

        private const string Name = "SShNetSftp";

        private readonly ILogger<SshNetSftp> _logger;

        public SshNetSftp(ILogger<SshNetSftp> logger)
        {
            _logger = logger;
        }

        private SftpClient CreateSftpClient()
        {
            if (_dictionary.TryGetValue(Name, out var sftp))
                return sftp;


            var config = AppConfig.Default.Deploy;

            var connectionInfo = new ConnectionInfo(config.Host,
                config.Port,
                config.Root,
                new PasswordAuthenticationMethod(config.Root, config.Password));

            var client = new SftpClient(connectionInfo);
            _dictionary.TryAdd(Name, client);
            return client;
        }


        public void SyncTreeUpload(string remotePath, string localPath)
        {
            Utils.TryCatchAction(() =>
            {
                var directory = new List<string>(Directory.GetDirectories(localPath, "*", SearchOption.AllDirectories));

                using var sftp = CreateSftpClient();
                sftp.Connect();
                _logger.LogInformation("【创建sfp链接成功】....");

                //todo 创建目录
                _logger.LogInformation("开始检查目录 ...");
                if (!sftp.Exists(remotePath))
                {
                    sftp.CreateDirectory(remotePath);
                    _logger.LogInformation($"远程目录  {remotePath}   创建成功 ...");
                }

                sftp.SynchronizeDirectories(localPath, remotePath, "*.*");
                _logger.LogInformation($"本地目录  {localPath}   同步远程目录 {remotePath} 成功 ...");

                directory.ForEach(item =>
                {
                    var path = item.Replace(localPath, remotePath);
                    path = path.Replace("\\", "/");
                    if (!sftp.Exists(path))
                    {
                        sftp.CreateDirectory(path);
                        _logger.LogInformation($"远程目录  {path}   创建成功 ...");
                    }

                    sftp.SynchronizeDirectories(item, path, "*.*");
                    _logger.LogInformation($"本地目录  {item}   同步远程目录 {path} 成功 ...");
                });

                _dictionary.Remove(Name);
            });
        }
    }
}