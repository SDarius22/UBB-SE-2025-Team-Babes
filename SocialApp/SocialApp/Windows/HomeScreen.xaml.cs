using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SocialApp.Pages;
using SocialApp.Windows;
using Microsoft.UI.Xaml.Navigation;
using SocialApp.Pages;
using SocialApp.Windows;
using System;

namespace SocialApp
{
    public sealed partial class HomeScreen : Page
    {
        private AppController controller;

        public HomeScreen()
        {
            this.InitializeComponent();
            SetNavigation();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is AppController controller)
            {
                this.controller = controller;
                TopBar.SetController(controller);
            }
        }

        private void SetNavigation()
        {
            TopBar.HomeButtonInstance.Click += HomeClick;
            TopBar.UserButtonInstance.Click += UserClick;
            TopBar.GroupsButtonInstance.Click += GroupsClick;
        }

        private void HomeClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HomeScreen), controller);
        }

        private void GroupsClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GroupsScreen), controller);
        }

        private void UserClick(object sender, RoutedEventArgs e)
        {
            if (IsLoggedIn())
            {
                Frame.Navigate(typeof(UserPage), controller);
            }
            else
            {
                Frame.Navigate(typeof(LoginRegisterPage), controller);
            }
        }

        private bool IsLoggedIn()
        {
            return controller.CurrentUser != null;
        }
    }
}
