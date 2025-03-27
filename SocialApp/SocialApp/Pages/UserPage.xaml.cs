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
using SocialApp.Pages;
using SocialApp.Services;
using SocialApp.Repository;
using SocialApp.Components;
using SocialApp.Entities;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SocialApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserPage : Page
    {

        private AppController controller;
        private UserRepository userRepository;
        private UserService userService;
        private PostRepository postRepository;
        private PostService postService;
        private GroupRepository groupRepository;

        public UserPage()
        {
            this.InitializeComponent();
            SetNavigation();
            SetContent();
            SetPostsContent();

            userRepository = new UserRepository();
            userService = new UserService(userRepository);
            postRepository = new PostRepository();
            groupRepository = new GroupRepository();
            postService = new PostService(postRepository, userRepository, groupRepository);


            this.Loaded += SetContent;
            this.Loaded += PostsClick;
            this.Loaded += SetNavigation;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is AppController controller)
            {
                this.controller = controller;
            }
        }

        private void SetNavigation(object sender, RoutedEventArgs e)
        {
            TopBar.HomeButtonInstance.Click += HomeClick;
            TopBar.UserButtonInstance.Click += UserClick;
            TopBar.GroupsButtonInstance.Click += GroupsClick;
        }

        private void HomeClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HomeScreen), controller);
        }

        private void GroupsClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GroupsScreen), controller);
        }

        private void UserClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UserPage), controller);
        }

        private async void SetContent(object sender, RoutedEventArgs e)
        {
            if (controller.CurrentUser != null)
            {
                if (controller.CurrentUser.Image != string.Empty)
                    ProfileImage.Source = await AppController.DecodeBase64ToImageAsync(controller.CurrentUser.Image);
                Username.Text = controller.CurrentUser.Username;
                FollowLogOutButton.Content = "Logout";
                FollowLogOutButton.Click += Logout;
                SetPostsContent(sender, e);
            }
            else
            {
                FollowLogOutButton.Content = IsFollowed() ? "Unfollow" : "Follow";
            }

        }

        private bool IsFollowed()
        {
            return false;
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            controller.Logout();
            Frame.Navigate(typeof (HomeScreen), controller);
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
            FollowersStack.Visibility = Visibility.Collapsed;

            PopulateFeed();
        }

        private void PopulateFeed()
        {
            PostsFeed.ClearPosts();

            List<Post> userPosts = postService.GetByUserId(controller.CurrentUser.Id);

            foreach (Post post in userPosts)
            {
                PostsFeed.AddPost(new PostComponent(post.Title, post.Visibility, post.UserId, post.Content, post.CreatedDate));
            }


            PostsFeed.Visibility = Visibility.Visible;

            PostsFeed.DisplayCurrentPage();

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
            FollowersStack.Visibility = Visibility.Collapsed;
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
            FollowersStack.Visibility = Visibility.Collapsed;
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
            FollowersStack.Visibility = Visibility.Visible;

            PopulateFollowers();
        }

        private void PopulateFollowers()
        {
            FollowersStack.Children.Clear();

            List<User> followers = userService.GetUserFollowers(controller.CurrentUser.Id);

            foreach (User user in followers)
            {
                FollowersStack.Children.Add(new Follower(user.Username, userService.GetUserFollowing(controller.CurrentUser.Id).Contains(user), user.Id));
            }
        }
    }
}
