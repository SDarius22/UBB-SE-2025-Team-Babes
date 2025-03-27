using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SocialApp.Entities;
using SocialApp.Repository;
using SocialApp.Services;
using System.Collections.Generic;

namespace SocialApp.Components
{
    public sealed partial class Follower : UserControl
    {
        private User user;
        private bool followed;

        private AppController controller;
        private UserRepository userRepository;
        private UserService userService;
        private PostRepository postRepository;
        private PostService postService;
        private GroupRepository groupRepository;

        public Follower(string username, bool followed, User user, AppController controller)
        {
            this.user = user;
            this.followed = followed;
            this.controller = controller;

            this.InitializeComponent();

            userRepository = new UserRepository();
            userService = new UserService(userRepository);
            postRepository = new PostRepository();
            groupRepository = new GroupRepository();
            postService = new PostService(postRepository, userRepository, groupRepository);

            Name.Text = username;
            Button.Content = IsFollowed() ? "Unfollow" : "Follow";
        }

        private bool IsFollowed()
        {
            List<User> following = userService.GetUserFollowing(controller.CurrentUser.Id);
            foreach (User user in following)
            {
                if (user.Id == this.user.Id) return true;
            }

            return false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button.Content = Button.Content.ToString() == "Follow" ? "Unfollow" : "Follow";
            if (!IsFollowed())
            {
                userService.FollowUser(controller.CurrentUser.Id, user.Id);
            }
            else
            {
                userService.UnfollowUser(controller.CurrentUser.Id, user.Id);
            }
        }
    }
}