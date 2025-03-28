using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SocialApp.Services;
using SocialApp.Repository;
using SocialApp.Entities;
using SocialApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialApp.Pages
{
    public sealed partial class CreatePostPage : Page
    {
        private AppController _controller;
        private PostService _postService;
        private GroupService _groupService;
        private List<Group> _userGroups;

        public CreatePostPage()
        {
            InitializeComponent();
            InitializeServices();
            TitleInput.TextChanged += TitleInput_TextChanged;
            DescriptionInput.TextChanged += DescriptionInput_TextChanged;
        }

        private void InitializeServices()
        {
            var postRepository = new PostRepository();
            var userRepository = new UserRepository();
            var groupRepository = new GroupRepository();
            _postService = new PostService(postRepository, userRepository, groupRepository);
            _groupService = new GroupService(groupRepository, userRepository);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is AppController controller)
            {
                _controller = controller;
                TopBar.SetControllerAndFrame(_controller, Frame);
                LoadUserGroups();
                InitializeVisibilityOptions();
            }
        }

        private void LoadUserGroups()
        {
            _userGroups = _groupService.GetGroupsForUser(_controller.CurrentUser.Id);
            GroupsListBox.ItemsSource = _userGroups;
        }

        private void InitializeVisibilityOptions()
        {
            VisibilityComboBox.ItemsSource = Enum.GetValues(typeof(PostVisibility));
            VisibilityComboBox.SelectedIndex = 0;
        }

        private void TitleInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            TitleCharCounter.Text = $"{TitleInput.Text.Length}/50";
        }

        private void DescriptionInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            DescriptionCharCounter.Text = $"{DescriptionInput.Text.Length}/250";
        }

        private void VisibilityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GroupSelectionPanel.Visibility = ((PostVisibility)VisibilityComboBox.SelectedItem) == PostVisibility.Groups
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            // Your image handling logic
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void PostButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateInputs();
                var selectedVisibility = (PostVisibility)VisibilityComboBox.SelectedItem;
                var postsToCreate = CreatePosts(selectedVisibility);

                foreach (var post in postsToCreate)
                {
                    _postService.ValidateAdd(
                        post.Title,
                        post.Description,
                        post.UserId,
                        post.GroupId,
                        post.Visibility,
                        post.Tag
                    );
                }

                Frame.Navigate(typeof(UserPage), _controller);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(TitleInput.Text))
                throw new Exception("Title is required!");

            if (string.IsNullOrWhiteSpace(DescriptionInput.Text))
                throw new Exception("Content cannot be empty!");

            var selectedVisibility = (PostVisibility)VisibilityComboBox.SelectedItem;
            if (selectedVisibility == PostVisibility.Groups &&
               !GroupsListBox.SelectedItems.Any())
            {
                throw new Exception("Please select at least one group!");
            }
        }

        private List<Post> CreatePosts(PostVisibility visibility)
        {
            var posts = new List<Post>();
            var basePost = new Post
            {
                Title = TitleInput.Text.Trim(),
                Description = DescriptionInput.Text.Trim(),
                UserId = _controller.CurrentUser.Id,
                GroupId = -1, // Start with null for non-group posts
                CreatedDate = DateTime.Now,
                Visibility = visibility,
                Tag = GetSelectedTag()
            };

            if (visibility == PostVisibility.Groups)
            {
                // Ensure you are assigning a valid GroupId here
                if (GroupsListBox.SelectedItems.Count == 0)
                {
                    throw new Exception("Please select at least one group!");
                }

                foreach (Group group in GroupsListBox.SelectedItems)
                {
                    posts.Add(new Post
                    {
                        Title = basePost.Title,
                        Description = basePost.Description,
                        UserId = basePost.UserId,
                        GroupId = group.Id, // Assign valid GroupId here
                        CreatedDate = basePost.CreatedDate,
                        Visibility = PostVisibility.Groups, // Maintain Groups visibility
                        Tag = basePost.Tag
                    });
                }
            }
            else
            {
                posts.Add(basePost);
            }

            return posts;
        }


        private PostTag GetSelectedTag()
        {
            if (MiscRadioButton.IsChecked == true) return PostTag.Misc;
            if (FoodRadioButton.IsChecked == true) return PostTag.Food;
            return PostTag.Workout;
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }
    }
}