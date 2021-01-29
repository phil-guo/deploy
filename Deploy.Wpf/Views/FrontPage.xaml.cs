using System;
using System.Windows;
using Deploy.Appliction.Config;

namespace Deploy.Wpf.Views
{
    public partial class FrontPage
    {
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
            Display.SelectedText = $"你好{Environment.NewLine}";
            Display.SelectedText += $"不，我不好{Environment.NewLine}";
        }
    }
}