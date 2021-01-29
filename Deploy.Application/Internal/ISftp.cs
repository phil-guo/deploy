namespace Deploy.Appliction.Internal
{
    public interface ISftp
    {
        /// <summary>
        /// 上传-同步目录树
        /// </summary>
        /// <param name="remotePath"></param>
        /// <param name="localPath"></param>
        void SyncTreeUpload(string remotePath, string localPath);

        /// <summary>
        /// 文件目录是否存在
        /// </summary>
        /// <returns></returns>
        bool FileDirectoryExists(string path);

        /// <summary>
        /// 创建文件目录
        /// </summary>
        /// <param name="path"></param>
        void CreateFileDirectory(string path);
    }
}