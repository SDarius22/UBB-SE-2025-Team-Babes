using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SocialApp.Repository;
using SocialApp.Services;
using SocialApp.Components;
using SocialApp.Entities;

namespace SocialApp.Pages
{
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
        private int GroupId;
        private Entities.Group group;

        public GroupPage(int groupId)
        {
            this.InitializeComponent();
            this.Loaded += DisplayPage;
            GroupId = groupId;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is AppController controller)
            {
                this.controller = controller;
                TopBar.SetControllerAndFrame(controller, this.Frame);
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
            group = groupService.GetById(GroupId);

            SetVisibilities();
            SetContent();
            PopulateMembers();
        }

        private void SetVisibilities()
        {
            bool isAdmin = UserIsAdmin();
            EditGroupButton.Visibility = isAdmin ? visible : collapsed;
        }

        private bool UserIsAdmin()
        {
            return groupRepository.GetById(GroupId).AdminId == controller.CurrentUser.Id;
        }

        private async void SetContent()
        {
            GroupTitle.Text = group.Name;
            GroupDescription.Text = group.Description;
            if (!string.IsNullOrEmpty(group.Image))
                GroupImage.Source = await AppController.DecodeBase64ToImageAsync(group.Image);
            PopulateFeed();
        }

        private void PopulateFeed()
        {
            PostsFeed.ClearPosts();
            List<Post> groupPosts = postService.GetByGroupId(GroupId);
            foreach (Post post in groupPosts)
            {
                PostsFeed.AddPost(new PostComponent(post.Title, post.Visibility, post.UserId, post.Content, post.CreatedDate));
            }
            PostsFeed.Visibility = Visibility.Visible;
            PostsFeed.DisplayCurrentPage();
        }

        private void PopulateMembers()
        {
            MembersList.Children.Clear();
            bool isAdmin = UserIsAdmin();
            List<User> members = groupService.GetUsersFromGroup(GroupId);
            foreach (User member in members)
            {
                MembersList.Children.Add(new Member(member, controller, this.Frame, GroupId, isAdmin));
            }
        }
    }
}