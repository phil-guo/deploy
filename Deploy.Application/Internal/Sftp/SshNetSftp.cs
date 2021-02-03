using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Deploy.Appliction.Config;
using Deploy.Appliction.Extensions;
using Renci.SshNet;

namespace Deploy.Appliction.Internal.Sftp
{
    public class SshNetSftp : ISftp
    {
        public Dictionary<string, SftpClient> Dictionary = new Dictionary<string, SftpClient>();

        private const string Name = "SShNetSftp";

        public SftpClient CreateSftpClient()
        {
            if (Dictionary.TryGetValue(Name, out var sftp))
                return sftp;


            var config = AppConfig.Default.Deploy;

            var connectionInfo = new ConnectionInfo(config.Host,
                config.Port,
                config.Root,
                new PasswordAuthenticationMethod(config.Root, config.Password));

            var client = new SftpClient(connectionInfo);
            Dictionary.TryAdd(Name, client);
            return client;
        }


        public void SyncTreeUpload(string remotePath, string localPath)
        {
            Utils.TryCatchAction(() =>
            {
                var directory = new List<string>(Directory.GetDirectories(localPath, "*", SearchOption.AllDirectories));


                using var sftp = CreateSftpClient();
                sftp.Connect();

                //todo 创建目录
                if (!sftp.Exists(remotePath))
                    sftp.CreateDirectory(remotePath);

                sftp.SynchronizeDirectories(localPath, remotePath, "*.*");

                directory.ForEach(item =>
                {
                    var path = item.Replace(localPath, remotePath);
                    path = path.Replace("\\", "/");
                    if (!sftp.Exists(path))
                        sftp.CreateDirectory(path);

                    sftp.SynchronizeDirectories(item, path, "*.*");
                });
            });
        }

        public bool FileDirectoryExists(string path)
        {
            throw new NotImplementedException();
        }

        public void CreateFileDirectory(string path)
        {
            throw new NotImplementedException();
        }
    }
}
