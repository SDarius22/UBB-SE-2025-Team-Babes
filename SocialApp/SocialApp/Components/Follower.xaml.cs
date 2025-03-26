using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SocialApp.Repository;
using SocialApp.Services;

namespace SocialApp.Components
{
    public sealed partial class Follower : UserControl
    {
        private long userId;
        private bool followed;

        private AppController controller;
        private UserRepository userRepository;
        private UserService userService;
        private PostRepository postRepository;
        private PostService postService;
        private GroupRepository groupRepository;

        public Follower(string username, bool followed, long userId)
        {
            this.InitializeComponent();

            userRepository = new UserRepository();
            userService = new UserService(userRepository);
            postRepository = new PostRepository();
            groupRepository = new GroupRepository();
            postService = new PostService(postRepository, userRepository, groupRepository);

            Name.Text = username;
            Button.Content = followed ? "Unfollow" : "Follow";
            this.userId = userId;
            this.followed = followed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button.Content = Button.Content.ToString() == "Follow" ? "Unfollow" : "Follow";
            if (!followed)
            {
                userService.FollowUser(controller.CurrentUser.Id, userId);
            }
            else
            {
                 // unfollow
            }
            followed = !followed;
        }
    }
}