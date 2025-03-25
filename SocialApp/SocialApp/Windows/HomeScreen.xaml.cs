using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SocialApp.Pages;
using SocialApp.Windows;

namespace SocialApp
{
    public sealed partial class HomeScreen : Page
    {
        public HomeScreen()
        {
            this.InitializeComponent();
            SetNavigation();
        }

        private void SetNavigation()
        {
            TopBar.HomeButtonInstance.Click += HomeClick;
            TopBar.UserButtonInstance.Click += UserClick;
            TopBar.GroupsButtonInstance.Click += GroupsClick;
        }

        private void HomeClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HomeScreen));
        }

        private void GroupsClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GroupsScreen));
        }

        private void UserClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UserPage));
        }
    }
}
