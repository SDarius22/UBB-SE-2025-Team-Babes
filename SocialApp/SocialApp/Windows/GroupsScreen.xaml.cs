using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SocialApp.Components;
using SocialApp.Pages;

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
            GroupsDrawer.NavigationFrame = this.Frame;
            SetNavigation();
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

            var controller = App.Services.GetService<AppController>();
            TopBar.SetFrame(this.Frame);
            TopBar.SetGroups();
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