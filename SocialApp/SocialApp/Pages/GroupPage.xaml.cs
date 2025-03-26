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
using SocialApp.Repository;
using SocialApp.Services;
using SocialApp.Components;
using SocialApp.Entities;

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
        private AppController controller;
        private UserRepository userRepository;
        private UserService userService;
        private PostRepository postRepository;
        private PostService postService;
        private GroupRepository groupRepository;
        private GroupService groupService;

        public long GroupId { get; set; }

        public GroupPage()
        {
            this.InitializeComponent();
            this.Loaded += DisplayPage;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is AppController controller)
            {
                this.controller = controller;
            }
        }

        private void DisplayPage(object sender, RoutedEventArgs e)
        {
            userRepository = new UserRepository();
            userService = new UserService(userRepository);
            groupRepository = new GroupRepository();
            groupService = new GroupService(groupRepository, userRepository);
            postRepository = new PostRepository();
            postService = new PostService(postRepository, userRepository, groupRepository);

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
            if (IsLoggedIn())
            {
                Frame.Navigate(typeof(UserPage));
            }
            else
            {
                Frame.Navigate(typeof(LoginRegisterPage));
            }
        }

        private bool IsLoggedIn()
        {
            return controller.CurrentUser != null;
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
            return groupRepository.GetById(GroupId).AdminId == controller.CurrentUser.Id;
        }

        private void SetRemoveButtonsVisible()
        {

        }

        private void SetRemoveButtonsCollapsed()
        {

        }

        private void SetContent()
        {
            // set details;
            PopulateFeed();
        }
        private void PopulateFeed()
        {

            List<Post> groupPosts = postService.GetByGroupId(GroupId);

            foreach (Post post in groupPosts)
            {
                PostsFeed.AddPost(new PostComponent(post.Title, post.Visibility, post.UserId, post.Content, post.CreatedDate));
            }


            PostsFeed.Visibility = Visibility.Visible;

            PostsFeed.DisplayCurrentPage();

        }
    }
}
