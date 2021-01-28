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
            UserName.Text = AppConfig.Default.Deploy.Root;
            RemotePath.Text = AppConfig.Default.Deploy.RemotePath;
        
            Display.SelectedText = "首都基辅罗斯的1111fsfa开发大黄蜂卡号打卡顺丰航空的罚款的撒旦雷锋精神劳动法爱的弗拉斯卡少了几分拉萨的飞机阿斯弗的啦师父的就爱上了反对" + Environment.NewLine;
            Display.SelectedText += "2222222222222222士大夫是";
        }

        private void Deploy_Click(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}