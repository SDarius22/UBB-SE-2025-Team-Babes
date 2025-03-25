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
using Windows.UI;
using System.Drawing;
using SocialApp.Windows;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SocialApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserPage : Page
    {

        public UserPage()
        {
            this.InitializeComponent();
            SetNavigation();
            SetContent();
            SetPostsContent();
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

        private void SetContent()
        {
            FollowLogOutButton.Content = IsLoggedIn() ? "Log Out" : (IsFollowed() ? "Unfollow" : "Follow");
        }

        private bool IsLoggedIn()
        {
            return false;
        }

        private bool IsFollowed()
        {
            return false;
        }

        private void PostsClick(object sender, RoutedEventArgs e)
        {
            SetPostsContent();
        }

        private void SetPostsContent()
        {
            PostsButton.IsEnabled = false;
            WorkoutsButton.IsEnabled = true;
            MealsButton.IsEnabled = true;
            FollowersButton.IsEnabled = true;
        }

        private void WorkoutsClick(object sender, RoutedEventArgs e)
        {
            SetWorkoutsContent();
        }

        private void SetWorkoutsContent()
        {
            PostsButton.IsEnabled = true;
            WorkoutsButton.IsEnabled = false;
            MealsButton.IsEnabled = true;
            FollowersButton.IsEnabled = true;
        }

        private void MealsClick(object sender, RoutedEventArgs e)
        {
            SetMealsContent();
        }

        private void SetMealsContent()
        {
            PostsButton.IsEnabled = true;
            WorkoutsButton.IsEnabled = true;
            MealsButton.IsEnabled = false;
            FollowersButton.IsEnabled = true;
        }

        private void FollowersClick(object sender, RoutedEventArgs e)
        {
            SetFollowersContent();
        }

        private void SetFollowersContent()
        {
            PostsButton.IsEnabled = true;
            WorkoutsButton.IsEnabled = true;
            MealsButton.IsEnabled = true;
            FollowersButton.IsEnabled = false;
        }
    }
}
