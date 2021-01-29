using System;
using System.IO;
using System.Windows;
using Autofac;
using Deploy.Appliction.Config;
using Deploy.Appliction.Internal;
using Microsoft.Extensions.Logging;

namespace Deploy.Wpf.Views
{
    public partial class FrontPage
    {
        // private readonly ISftp _sftp;
        private readonly ISsh _ssh;

        public FrontPage()
        {
            InitializeComponent();

            Init(AppConfig.Default.Deploy);
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

        private void Deploy_Click(object sender, RoutedEventArgs e)
        {
            var sftp = Appliction.Extensions.Utils.Current.Resolve<ISftp>();

            if (!FileExists())
                return;
        }

        private bool FileExists()
        {
            var frontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? string.Empty, "Config", "front");
            if (!Directory.Exists(frontPath))
                Directory.CreateDirectory(frontPath);

            //todo 判断dockerfile文件跟 nginx 配置文件是否存在
            if (!File.Exists(Path.Combine(frontPath, "Dockerfile")))
            {
                Display.SelectedText += $"dockerfile文件不存在{Environment.NewLine}";
                return false;
            }

            if (!File.Exists(Path.Combine(frontPath, "nginx.conf")))
            {
                Display.SelectedText += $"nginx.conf文件不存在{Environment.NewLine}";
                return false;
            }

            return true;
        }
    }
}