using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using SocialApp.Services;
using SocialApp.Repository;
using SocialApp.Components;
using SocialApp.Entities;
using System.Linq;

namespace SocialApp.Pages
{
    public sealed partial class UserPage : Page
    {
        private AppController controller;
        private UserRepository userRepository;
        private UserService userService;
        private PostRepository postRepository;
        private PostService postService;
        private GroupRepository groupRepository;
        private User displayedUser; // User to display (may differ from CurrentUser)

        public UserPage()
        {
            this.InitializeComponent();

            userRepository = new UserRepository();
            userService = new UserService(userRepository);
            postRepository = new PostRepository();
            groupRepository = new GroupRepository();
            postService = new PostService(postRepository, userRepository, groupRepository);

            this.Loaded += SetContent;
            this.Loaded += PostsClick;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is UserPageNavigationArgs args)
            {
                this.controller = args.Controller;
                this.displayedUser = args.SelectedUser;
                TopBar.SetControllerAndFrame(controller, this.Frame);
            }
            else if (e.Parameter is AppController controller)
            {
                this.controller = controller;
                this.displayedUser = controller.CurrentUser; // Default to CurrentUser if no specific user provided
                TopBar.SetControllerAndFrame(controller, this.Frame);
            }
        }

        private async void SetContent(object sender, RoutedEventArgs e)
        {
            if (displayedUser != null)
            {
                if (!string.IsNullOrEmpty(displayedUser.Image))
                    ProfileImage.Source = await AppController.DecodeBase64ToImageAsync(displayedUser.Image);
                Username.Text = displayedUser.Username;

                if (controller.CurrentUser != null && controller.CurrentUser.Id == displayedUser.Id)
                {
                    FollowLogOutButton.Content = "Logout";
                    FollowLogOutButton.Click -= Logout;
                    FollowLogOutButton.Click += Logout;
                }
                else
                {
                    FollowLogOutButton.Content = IsFollowed() ? "Unfollow" : "Follow";
                    FollowLogOutButton.Click -= FollowUnfollow;
                    FollowLogOutButton.Click += FollowUnfollow;
                }
                SetPostsContent(sender, e);
            }
            else
            {
                FollowLogOutButton.Content = "Follow";
            }
        }

        private bool IsFollowed()
        {
            // Check if CurrentUser follows displayedUser
            return controller?.CurrentUser != null && userService.GetUserFollowing(controller.CurrentUser.Id).Any(u => u.Id == displayedUser.Id);
        }

        private void FollowUnfollow(object sender, RoutedEventArgs e)
        {
            if (controller?.CurrentUser != null && displayedUser != null)
            {
                if (IsFollowed())
                {
                    userService.UnfollowUser(controller.CurrentUser.Id, displayedUser.Id);
                    FollowLogOutButton.Content = "Follow";
                }
                else
                {
                    userService.FollowUser(controller.CurrentUser.Id, displayedUser.Id);
                    FollowLogOutButton.Content = "Unfollow";
                }
            }
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            controller.Logout();
            Frame.Navigate(typeof(HomeScreen), controller);
        }

        private void PostsClick(object sender, RoutedEventArgs e)
        {
            SetPostsContent(sender, e);
            PostsFeed.DisplayCurrentPage();
        }

        private void SetPostsContent(object sender, RoutedEventArgs e)
        {
            PostsButton.IsEnabled = false;
            WorkoutsButton.IsEnabled = true;
            MealsButton.IsEnabled = true;
            FollowersButton.IsEnabled = true;

            PostsFeed.Visibility = Visibility.Visible;
            FollowersScrollViewer.Visibility = Visibility.Collapsed;

            PopulateFeed();
        }

        private void PopulateFeed()
        {
            PostsFeed.ClearPosts();

            if (displayedUser != null)
            {
                List<Post> userPosts = postService.GetByUserId(displayedUser.Id);
                foreach (Post post in userPosts)
                {
                    PostsFeed.AddPost(new PostComponent(post.Title, post.Visibility, post.UserId, post.Description, post.CreatedDate, post.Id));
                }
                PostsFeed.DisplayCurrentPage();
            }
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

            PostsFeed.Visibility = Visibility.Collapsed;
            FollowersScrollViewer.Visibility = Visibility.Collapsed;
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

            PostsFeed.Visibility = Visibility.Collapsed;
            FollowersScrollViewer.Visibility = Visibility.Collapsed;
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

            PostsFeed.Visibility = Visibility.Collapsed;
            FollowersScrollViewer.Visibility = Visibility.Visible;

            PopulateFollowers();
        }

        private void PopulateFollowers()
        {
            FollowersStack.Children.Clear();

            if (displayedUser != null)
            {
                List<User> followers = userService.GetUserFollowers(displayedUser.Id);
                foreach (User user in followers)
                {
                    FollowersStack.Children.Add(new Follower(user.Username, userService.GetUserFollowing(controller.CurrentUser?.Id ?? -1).Contains(user), user, controller, this.Frame));
                }
            }
        }
    }
}