using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SocialApp.Components;
using SocialApp.Pages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SocialApp.Windows
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GroupsScreen : Page
    {
        private AppController controller;

        public GroupsScreen()
        {
            this.InitializeComponent();
            SetNavigation();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is AppController controller)
            {
                this.controller = controller;
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
