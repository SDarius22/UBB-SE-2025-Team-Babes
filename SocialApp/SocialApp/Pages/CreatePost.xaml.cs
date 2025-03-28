using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using SocialApp.Components;
using SocialApp.Entities;
using SocialApp.Enums;
using SocialApp.Repository;
using SocialApp.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static System.Net.Mime.MediaTypeNames;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Storage;
using Group = SocialApp.Entities.Group;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SocialApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreatePost : Page
    {
        private AppController _controller;
        private PostService _postService;
        private GroupService _groupService;
        private List<Entities.Group> _userGroups;
        private String image = string.Empty;
        public CreatePost()
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
            _controller = App.Services.GetService<AppController>();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TopBar.SetFrame(Frame);
            TopBar.SetCreate();
            LoadUserGroups();
            InitializeVisibilityOptions();
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

        private async void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var picker = new FileOpenPicker
                {
                    ViewMode = PickerViewMode.Thumbnail,
                    SuggestedStartLocation = PickerLocationId.PicturesLibrary
                };
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");
                picker.FileTypeFilter.Add(".png");

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.CurrentWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

                StorageFile file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    image = "image://" + await AppController.EncodeImageToBase64Async(file);
                    DescriptionInput.Text = "";
                    Confirmation.Text = "Image uploaded.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = $"Error uploading image: {ex.Message}";
            }
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
                _postService.ValidateAdd(TitleInput.Text, DescriptionInput.Text == "" ? image : DescriptionInput.Text, _controller.CurrentUser.Id, 0, selectedVisibility, GetSelectedTag());
                Frame.Navigate(typeof(HomeScreen));
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

            if (image == string.Empty && string.IsNullOrWhiteSpace(DescriptionInput.Text))
                throw new Exception("Content cannot be empty!");

            var selectedVisibility = (PostVisibility)VisibilityComboBox.SelectedItem;
            if (selectedVisibility == PostVisibility.Groups &&
               !GroupsListBox.SelectedItems.Any())
            {
                throw new Exception("Please select at least one group!");
            }
        }

        //private List<Post> CreatePosts(PostVisibility visibility)
        //{
        //    var posts = new List<Post>();
        //    var basePost = new Post
        //    {
        //        Title = TitleInput.Text.Trim(),
        //        Content = DescriptionInput.Text.Trim(),
        //        UserId = _controller.CurrentUser.Id,
        //        GroupId = -1, // Start with null for non-group posts
        //        CreatedDate = DateTime.Now,
        //        Visibility = visibility,
        //        Tag = GetSelectedTag()
        //    };

        //    if (visibility == PostVisibility.Groups)
        //    {
        //        // Ensure you are assigning a valid GroupId here
        //        if (GroupsListBox.SelectedItems.Count == 0)
        //        {
        //            throw new Exception("Please select at least one group!");
        //        }

        //        foreach (Group group in GroupsListBox.SelectedItems)
        //        {
        //            posts.Add(new Post
        //            {
        //                Title = basePost.Title,
        //                Content = basePost.Content,
        //                UserId = basePost.UserId,
        //                GroupId = group.Id, // Assign valid GroupId here
        //                CreatedDate = basePost.CreatedDate,
        //                Visibility = PostVisibility.Groups, // Maintain Groups visibility
        //                Tag = basePost.Tag
        //            });
        //        }
        //    }
        //    else
        //    {
        //        posts.Add(basePost);
        //    }

        //    return posts;
        //}


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
