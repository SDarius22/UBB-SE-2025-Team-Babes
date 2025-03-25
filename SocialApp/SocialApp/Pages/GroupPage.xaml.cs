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
using SocialApp.Windows;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SocialApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GroupPage : Page
    {
        private const Visibility collapsed = Visibility.Collapsed;
        private const Visibility visible = Visibility.Visible;

        public GroupPage()
        {
            this.InitializeComponent();
            SetNavigation();
            SetVisibilities();
            SetContent();
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

        private void SetVisibilities()
        {
            if (UserIsAdmin())
            {
                EditGroupButton.Visibility = visible;
                SetRemoveButtonsVisible();
            }
            else
            {
                EditGroupButton.Visibility = collapsed;
                SetRemoveButtonsCollapsed();
            }
        }

        private bool UserIsAdmin()
        {
            return false;
        }

        private void SetRemoveButtonsVisible()
        {

        }

        private void SetRemoveButtonsCollapsed()
        {

        }

        private void SetContent()
        {

        }
    }
}
