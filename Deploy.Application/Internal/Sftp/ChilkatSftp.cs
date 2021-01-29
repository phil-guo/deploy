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
            {
                sftp = CreateSftp();
                SftpDictionary.TryAdd(Name, sftp);
            }

            if (sftp != null)
                _logger.LogInformation("ftp 链接创建成功");
            else
            {
                SftpDictionary.TryRemove(Name, out sftp);
                SftpDictionary.TryAdd(Name, CreateSftp());
            }
        }

        private SFtp CreateSftp()
        {
            var sftp = new Chilkat.SFtp();
            var config = AppConfig.Default.Deploy;

            var success = sftp.Connect(config.Host, config.Port);
            if (!success)
                _logger.LogInformation(sftp.LastErrorText);

            success = sftp.AuthenticatePw(config.Root, config.Password);

            if (!success)
            {
                _logger.LogInformation(sftp.LastErrorText);
                return null;
            }

            success = sftp.InitializeSftp();
            if (!success)
            {
                _logger.LogInformation(sftp.LastErrorText);
                return null;
            }

            return sftp;
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
            {
                _logger.LogInformation(sftp.LastErrorText);
                return;
            }

            _logger.LogInformation("目录同步成功,开始执行docker命令 ...");
        }

        public bool FileDirectoryExists(string path)
        {
            //返回值是以下值之一：
            // -1：无法检查。检查LastErrorText以确定失败的原因。
            // 0：文件不存在。
            // 1：常规文件存在。
            // 2：存在，但它是目录。
            // 3：存在，但它是一个符号链接（仅当followLinks为false时才可能）
            // 4：存在，但它是一种特殊的文件系统条目类型。
            // 5：它存在，但是它是一个未知的文件系统条目类型。
            // 6：存在，但是它是套接字文件系统条目类型。
            // 7：存在，但是它是字符设备条目类型。
            // 8：存在，但是它是块设备条目类型。
            // 9：存在，但是它是FIFO条目类型

            SftpDictionary.TryGetValue(Name, out var sftp);
            if (sftp == null)
            {
                _logger.LogInformation("sftp 没有创建链接");
                return false;
            }

            var followLinks = true;
            var status = sftp.FileExists(path, followLinks);

            if (status == 2)
                _logger.LogInformation("远程目录存在，开始同步本地目录 ...");
            return status == 2;
        }

        public void CreateFileDirectory(string path)
        {
            SftpDictionary.TryGetValue(Name, out var sftp);
            if (sftp == null)
            {
                _logger.LogInformation("sftp 没有创建链接");
                return;
            }

            var success = sftp.CreateDir(path);

            if (!success)
            {
                _logger.LogInformation(sftp.LastErrorText);
                return;
            }

            _logger.LogInformation("远程文件目录创建成功，开始同步本地目录 ...");
        }
    }
}