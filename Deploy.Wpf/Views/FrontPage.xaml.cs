using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Deploy.Appliction.Config;
using Deploy.Appliction.Extensions;
using Deploy.Appliction.Internal;
using Microsoft.Extensions.Logging;

namespace Deploy.Wpf.Views
{
    public partial class FrontPage
    {
        public Action<string> FrontTextBoxCallback;
        private readonly ILogger<FrontPage> _logger;

        public FrontPage()
        {
            InitializeComponent();

            Init(AppConfig.Default.Deploy);

            _logger = Utils.Current.Resolve<ILogger<FrontPage>>();

            Utils.TextBoxCallback = DispatcherInvoke;
        }

        public void Init(DeployOption deploy)
        {
            Host.Text = deploy.Host;
            Port.Text = deploy.MapperPort;
            UserName.Text = deploy.Root;
            RemotePath.Text = deploy.RemotePath;
            Password.Text = deploy.Password;
            Display.Text = "";
        }

        public void DispatcherInvoke(string appendText)
        {
            FrontTextBoxCallback = item => { Display.AppendText(appendText + Environment.NewLine); };

            Display.Dispatcher.Invoke(FrontTextBoxCallback, appendText);
        }

        public void Execute()
        {
            _logger.LogInformation("正在初始化 创建ssh,sftp 链接 ...");
            var sftp = Utils.Current.Resolve<ISftp>();
            var ssh = Utils.Current.Resolve<ISsh>();

            if (!FileExists())
                return;

            var config = AppConfig.Default.Deploy;

            //todo 创建远程目录
            if (!sftp.FileDirectoryExists(config.RemotePath))
                sftp.CreateFileDirectory(config.RemotePath);

            //todo 同步本地目录到远程目录
            sftp.SyncTreeUpload(config.RemotePath, config.LocalPath);
            
            //todo 开始执行shell 命令
            ssh.ExecuteFrontCmd("test","test");
        }

        private void Deploy_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = "";
            Task.Factory.StartNew(Execute);
        }

        private bool FileExists()
        {
            var frontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? string.Empty, "Config", "front");
            if (!Directory.Exists(frontPath))
                Directory.CreateDirectory(frontPath);

            //todo 判断dockerfile文件跟 nginx 配置文件是否存在
            if (!File.Exists(Path.Combine(frontPath, "Dockerfile")))
            {
                _logger.LogInformation("dockerfile文件不存在");
                return false;
            }

            if (!File.Exists(Path.Combine(frontPath, "nginx.conf")))
            {
                _logger.LogInformation("nginx.conf文件不存在");
                return false;
            }

            return true;
        }
    }
}