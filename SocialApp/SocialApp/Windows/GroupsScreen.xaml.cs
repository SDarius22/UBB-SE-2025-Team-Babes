using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SocialApp.Components;
using SocialApp.Pages;

namespace SocialApp.Windows
{
    public sealed partial class GroupsScreen : Page
    {
        private AppController controller;

        public GroupsScreen()
        {
            this.InitializeComponent();
            GroupsDrawer.NavigationFrame = this.Frame; // Pass Frame to GroupsDrawer
            SetNavigation(); // Set up TopBar navigation
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is AppController controller)
            {
                this.controller = controller;
                TopBar.SetControllerAndFrame(controller, this.Frame);
                GroupsDrawer.Controller = controller;
            }
            GroupsDrawer.NavigationFrame = this.Frame;
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

        private void UserClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UserPage));
        }

        private void GroupsClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GroupsScreen));
        }
    }
}