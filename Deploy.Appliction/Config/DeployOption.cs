namespace Deploy.Appliction.Config
{
    public class DeployOption
    {
        public string Host { get; set; } = "182.92.107.110";
        public int Port { get; set; } = 22;
        public int TimeOutMs { get; set; } = 5000;
        public string Root { get; set; } = "root";
        public string Password { get; set; } = "qwe123QWE";
        public string LocalPath { get; set; }
        public string RemotePath { get; set; } = "/home/wwwroot";
    }
}