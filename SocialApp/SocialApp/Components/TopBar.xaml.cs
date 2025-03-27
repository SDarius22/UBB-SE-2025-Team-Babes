using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SocialApp.Repository;
using SocialApp.Services;
using Windows.Networking.NetworkOperators;

namespace SocialApp.Components
{
    public sealed partial class TopBar : UserControl
    {

        private AppController controller;
        private UserRepository userRepository;
        private UserService userService;

        public TopBar()
        {
            this.InitializeComponent();
        }

        public void SetController(AppController controller)
        {
            this.controller = controller;
            SetPhoto();
        }

        private async void SetPhoto()
        {
            if (controller?.CurrentUser != null && !string.IsNullOrEmpty(controller.CurrentUser.Image))
            {
                UserImage.Source = await AppController.DecodeBase64ToImageAsync(controller.CurrentUser.Image);
            }
        }

        public Button HomeButtonInstance => HomeButton;
        public Button GroupsButtonInstance => GroupsButton;
        public Button CreatePostButtonInstance => CreatePostButton;
        public Button UserButtonInstance => UserButton;
    }
}
