using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autofac;
using Deploy.Appliction;
using MahApps.Metro.Controls;

namespace Deploy.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow :  MetroWindow
    {
        public MainWindow()
        {
            SetUpContainer();

            InitializeComponent();
        }

        private void SetUpContainer()
        {
            var builder = new ContainerBuilder();
            BuildUpContainer(builder);
            var container = builder.Build();
        }

        private void BuildUpContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<ApplicationModule>();
        }
    }
}