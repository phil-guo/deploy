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
    }
}