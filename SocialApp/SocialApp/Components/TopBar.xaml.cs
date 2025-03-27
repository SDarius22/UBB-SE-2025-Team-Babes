using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SocialApp.Pages;
using SocialApp.Windows;


namespace SocialApp.Components
{
    public sealed partial class TopBar : UserControl
    {
        private Frame frame;

        public TopBar()
        {
            this.InitializeComponent();
        }

        public void SetFrame(Frame frame)
        {
            this.frame = frame;
            SetPhoto();
            SetNavigationButtons();
        }

        private async void SetPhoto()
        {
            var controller = App.Services.GetService<AppController>();
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
        }

        private void HomeClick(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(HomeScreen));
        }

        private void GroupsClick(object sender, RoutedEventArgs e)
        {
            if (IsLoggedIn())
                frame.Navigate(typeof(GroupsScreen));
            else
                frame.Navigate(typeof(LoginRegisterPage));
        }

        private void UserClick(object sender, RoutedEventArgs e)
        {
            if (IsLoggedIn())
            {
                frame.Navigate(typeof(UserPage));
            }
            else
            {
                frame.Navigate(typeof(LoginRegisterPage));
            }
        }

        private bool IsLoggedIn()
        {
            var controller = App.Services.GetService<AppController>();
            return controller.CurrentUser != null;
        }

        public Button HomeButtonInstance => HomeButton;
        public Button GroupsButtonInstance => GroupsButton;
        public Button CreatePostButtonInstance => CreatePostButton;
        public Button UserButtonInstance => UserButton;
    }
}
