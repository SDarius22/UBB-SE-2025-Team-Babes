using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SocialApp.Pages;

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
        }

        private void HomeClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HomeScreen));
        }

        private void UserClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UserPage));
        }
    }
}
