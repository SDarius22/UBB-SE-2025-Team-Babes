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
    /// A page that displays groups and interacts with them.
    /// </summary>
    public sealed partial class GroupsScreen : Page
    {
        private AppController controller;
        private GroupsDrawer groupsDrawer;

        public GroupsScreen()
        {
            this.InitializeComponent();
            groupsDrawer = new GroupsDrawer(); // Initialize GroupsDrawer
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is AppController controller)
            {
                this.controller = controller;
                TopBar.SetControllerAndFrame(controller, this.Frame);
                groupsDrawer.SetControllerAndFrame(controller, this.Frame);
            }
        }

        private void CreateGroupButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsLoggedIn())
            {
                // Use this.Frame to navigate
                this.Frame.Navigate(typeof(CreateGroupPage), controller);
            }
            else
            {
                this.Frame.Navigate(typeof(LoginRegisterPage), controller);
            }
        }

        private bool IsLoggedIn()
        {
            return controller.CurrentUser != null;
        }

    }
}
