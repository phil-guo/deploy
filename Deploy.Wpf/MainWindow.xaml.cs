using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Autofac;
using Deploy.Appliction;
using MahApps.Metro.Controls;

namespace Deploy.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly Navigation.NavigationServiceEx navigationServiceEx;

        public MainWindow()
        {
            SetUpContainer();

            InitializeComponent();

            this.navigationServiceEx = new Navigation.NavigationServiceEx();
            this.navigationServiceEx.Navigated += this.NavigationServiceEx_OnNavigated;
            this.HamburgerMenuControl.Content = this.navigationServiceEx.Frame;

            // Navigate to the home page.
            this.Loaded += (sender, args) =>
                this.navigationServiceEx.Navigate(new Uri("Views/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void NavigationServiceEx_OnNavigated(object sender, NavigationEventArgs e)
        {
            // select the menu item
            this.HamburgerMenuControl.SelectedItem = this.HamburgerMenuControl
                .Items
                .OfType<VMs.MenuItem>()
                .FirstOrDefault(x => x.NavigationDestination == e.Uri);
            this.HamburgerMenuControl.SelectedOptionsItem = this.HamburgerMenuControl
                .OptionsItems
                .OfType<VMs.MenuItem>()
                .FirstOrDefault(x => x.NavigationDestination == e.Uri);

            // or when using the NavigationType on menu item
            // this.HamburgerMenuControl.SelectedItem = this.HamburgerMenuControl
            //                                              .Items
            //                                              .OfType<MenuItem>()
            //                                              .FirstOrDefault(x => x.NavigationType == e.Content?.GetType());
            // this.HamburgerMenuControl.SelectedOptionsItem = this.HamburgerMenuControl
            //                                                     .OptionsItems
            //                                                     .OfType<MenuItem>()
            //                                                     .FirstOrDefault(x => x.NavigationType == e.Content?.GetType());

            // update back button
            // this.GoBackButton.Visibility = this.navigationServiceEx.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
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

        private void LaunchGitHubSite(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "https://github.com/simple-gr/deploy");
        }

        private void HamburgerMenuControl_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs args)
        {
            if (args.InvokedItem is VMs.MenuItem menuItem && menuItem.IsNavigation)
            {
                this.navigationServiceEx.Navigate(menuItem.NavigationDestination);
            }
        }
    }
}