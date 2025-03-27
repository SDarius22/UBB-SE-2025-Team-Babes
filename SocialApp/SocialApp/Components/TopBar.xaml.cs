using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SocialApp.Pages;
using SocialApp.Repository;
using SocialApp.Services;
using SocialApp.Windows;
using Windows.Networking.NetworkOperators;

namespace SocialApp.Components
{
    public sealed partial class TopBar : UserControl
    {

        private AppController controller;
        private Frame frame;

        public TopBar()
        {
            this.InitializeComponent();
        }

        public void SetControllerAndFrame(AppController controller, Frame frame)
        {
            this.controller = controller;
            this.frame = frame;
            SetPhoto();
            SetNavigationButtons();
        }

        private async void SetPhoto()
        {
            if (controller?.CurrentUser != null && !string.IsNullOrEmpty(controller.CurrentUser.Image))
            {
                UserImage.Source = await AppController.DecodeBase64ToImageAsync(controller.CurrentUser.Image);
            }
        }

        private void SetNavigationButtons()
        {
            HomeButton.Click += HomeClick;
            UserButton.Click += UserClick;
            GroupsButton.Click += GroupsClick;
            CreatePostButton.Click += CreatePostButton_Click;
        }

        private void HomeClick(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(HomeScreen), controller);
        }

        private void GroupsClick(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(GroupsScreen), controller);
        }

        private void UserClick(object sender, RoutedEventArgs e)
        {
            if (IsLoggedIn())
            {
                frame.Navigate(typeof(UserPage), controller);
            }
            else
            {
                frame.Navigate(typeof(LoginRegisterPage), controller);
            }
        }

        private void CreatePostButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsLoggedIn())
            {
                frame.Navigate(typeof(CreatePostPage), controller);
            }
            else
            {
                frame.Navigate(typeof(LoginRegisterPage), controller);
            }
        }

        private bool IsLoggedIn()
        {
            return controller.CurrentUser != null;
        }

        public Button HomeButtonInstance => HomeButton;
        public Button GroupsButtonInstance => GroupsButton;
        public Button CreatePostButtonInstance => CreatePostButton;
        public Button UserButtonInstance => UserButton;
    }
}
