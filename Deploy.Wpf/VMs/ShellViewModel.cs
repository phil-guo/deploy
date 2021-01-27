using System;
using System.Collections.ObjectModel;
using Deploy.Wpf.Mvvm;
using Deploy.Wpf.Views;
using MahApps.Metro.IconPacks;

namespace Deploy.Wpf.VMs
{
    public class ShellViewModel : BindableBase
    {
        private static readonly ObservableCollection<MenuItem> AppMenu = new ObservableCollection<MenuItem>();
        private static readonly ObservableCollection<MenuItem> AppOptionsMenu = new ObservableCollection<MenuItem>();

        public ObservableCollection<MenuItem> Menu => AppMenu;

        public ObservableCollection<MenuItem> OptionsMenu => AppOptionsMenu;

        public ShellViewModel()
        {
            // Build the menus
            Menu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() {Kind = PackIconFontAwesomeKind.FontAwesomeBrands},
                Label = "前端",
                NavigationType = typeof(FrontPage),
                NavigationDestination = new Uri("Views/FrontPage.xaml", UriKind.RelativeOrAbsolute)
            });
            // this.Menu.Add(new MenuItem()
            // {
            //     Icon = new PackIconFontAwesome() {Kind = PackIconFontAwesomeKind.UserSolid},
            //     // Label = "User",
            //     // NavigationType = typeof(UserPage),
            //     // NavigationDestination = new Uri("Views/UserPage.xaml", UriKind.RelativeOrAbsolute)
            // });
            // this.Menu.Add(new MenuItem()
            // {
            //     Icon = new PackIconFontAwesome() {Kind = PackIconFontAwesomeKind.CoffeeSolid},
            //     // Label = "Break",
            //     // NavigationType = typeof(BreakPage),
            //     // NavigationDestination = new Uri("Views/BreakPage.xaml", UriKind.RelativeOrAbsolute)
            // });
            // this.Menu.Add(new MenuItem()
            // {
            //     Icon = new PackIconFontAwesome() {Kind = PackIconFontAwesomeKind.FontAwesomeBrands},
            //     // Label = "Awesome",
            //     // NavigationType = typeof(AwesomePage),
            //     // NavigationDestination = new Uri("Views/AwesomePage.xaml", UriKind.RelativeOrAbsolute)
            // });
            //
            // this.OptionsMenu.Add(new MenuItem()
            // {
            //     Icon = new PackIconFontAwesome() {Kind = PackIconFontAwesomeKind.CogsSolid},
            //     // Label = "Settings",
            //     // NavigationType = typeof(SettingsPage),
            //     // NavigationDestination = new Uri("Views/SettingsPage.xaml", UriKind.RelativeOrAbsolute)
            // });
            // this.OptionsMenu.Add(new MenuItem()
            // {
            //     Icon = new PackIconFontAwesome() {Kind = PackIconFontAwesomeKind.InfoCircleSolid},
            //     // Label = "About",
            //     // NavigationType = typeof(AboutPage),
            //     // NavigationDestination = new Uri("Views/AboutPage.xaml", UriKind.RelativeOrAbsolute)
            // });
        }
    }
}