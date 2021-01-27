using System.Collections.Concurrent;
using Chilkat;
using Deploy.Appliction.Config;
using Microsoft.Extensions.Logging;

namespace Deploy.Appliction.Internal.Sftp
{
    public class ChilkatSftp : ISftp
    {
        private const string Name = "sftp";

        public static readonly ConcurrentDictionary<string, Chilkat.SFtp> SftpDictionary =
            new ConcurrentDictionary<string, Chilkat.SFtp>();

        private readonly ILogger<ChilkatSftp> _logger;

        public ChilkatSftp(ILogger<ChilkatSftp> logger)
        {
            _logger = logger;

            if (!SftpDictionary.TryGetValue(Name, out var sftp))
                SftpDictionary.TryAdd(Name, CreateSftp());
        }

        private SFtp CreateSftp()
        {
            var sftp = new Chilkat.SFtp();

            var success = sftp.Connect(AppConfig.Default.Deploy.Host, AppConfig.Default.Deploy.Port);
            if (!success)
                _logger.LogInformation(sftp.LastErrorText);

            success = sftp.AuthenticatePw(AppConfig.Default.Deploy.Root, AppConfig.Default.Deploy.Password);

            if (!success)
                _logger.LogInformation(sftp.LastErrorText);

            success = sftp.InitializeSftp();
            if (!success)
                _logger.LogInformation(sftp.LastErrorText);

            return !success ? null : sftp;
        }

        public void SyncTreeUpload(string remotePath, string localPath)
        {
            SftpDictionary.TryGetValue(Name, out var sftp);
            if (sftp == null)
            {
                _logger.LogInformation("sftp 没有创建链接");
                return;
            }

            //  mode=0: Upload all files
            //  mode=1: Upload all files that do not exist on the server.
            //  mode=2: Upload newer or non-existant files.
            //  mode=3: Upload only newer files. If a file does not already exist on the server, it is not uploaded.
            //  mode=4: transfer missing files or files with size differences.
            //  mode=5: same as mode 4, but also newer files.
            var mode = 0;

            //  This example turns on recursion to synchronize the entire tree.
            //  Recursion can be turned off to synchronize the files of a single directory.
            var recursive = true;

            var success = sftp.SyncTreeUpload(localPath, remotePath, mode, recursive);

            if (!success)
                _logger.LogInformation(sftp.LastErrorText);
        }
    }
}