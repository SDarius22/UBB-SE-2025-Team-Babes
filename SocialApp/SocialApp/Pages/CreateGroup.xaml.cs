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
using SocialApp.Entities;
using SocialApp.Repository;
using SocialApp.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static System.Net.Mime.MediaTypeNames;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SocialApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateGroup : Page
    {
        private AppController _controller;
        private GroupService _groupService;
        private UserService _userService;
        private string image = string.Empty;
        public CreateGroup()
        {
            InitializeComponent();
            InitializeServices();
            GroupNameInput.TextChanged += GroupNameInput_TextChanged;
        }

        private void InitializeServices()
        {
            var groupRepository = new GroupRepository();
            var userRepository = new UserRepository();
            _groupService = new GroupService(groupRepository, userRepository);
            _userService = new UserService(userRepository);
            _controller = App.Services.GetService<AppController>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TopBar.SetFrame(Frame);
        }

        private void GroupNameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            GroupNameCharCounter.Text = $"{GroupNameInput.Text.Length}/55";
        }

        private void GroupDescriptionInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            GroupDescriptionCharCounter.Text = $"{GroupDescriptionInput.Text.Length}/250";
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void CreateGroupButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateInputs();

                var newGroup = new Entities.Group
                {
                    Name = GroupNameInput.Text.Trim(),
                    Description = string.IsNullOrWhiteSpace(GroupDescriptionInput.Text) ? null : GroupDescriptionInput.Text.Trim(),
                    AdminId = _controller.CurrentUser.Id,
                    Image = image
                };

                _groupService.ValidateAdd(newGroup.Name, newGroup.Description ?? "", newGroup.Image ?? "", newGroup.AdminId);
                Frame.Navigate(typeof(UserPage), _controller);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(GroupNameInput.Text))
                throw new Exception("Group name is required!");

            if (GroupNameInput.Text.Length > 55)
                throw new Exception("Group name cannot exceed 55 characters!");

            if (GroupDescriptionInput.Text.Length > 250)
                throw new Exception("Group description cannot exceed 250 characters!");
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }

        // User Search functionality
        private void UserSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var query = UserSearchBox.Text;

            if (string.IsNullOrWhiteSpace(query))
            {
                // Hide search results but don't affect layout
                UserSearchResults.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Show filtered results
                var users = _userService.GetUserFollowing(_controller.CurrentUser.Id)
                                        .Where(u => u.Username.Contains(query))
                                        .ToList();
                UserSearchResults.ItemsSource = users;
                UserSearchResults.Visibility = Visibility.Visible;
            }

            // Manually update layout (if needed)
            RootGrid.UpdateLayout();
        }



        // Handle selection of a user
        private void UserSearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserSearchResults.SelectedItem is User selectedUser)
            {
                AddUserToSelectedList(selectedUser);
            }
        }

        // Add user to the selected list (small version only)
        private void AddUserToSelectedList(User user)
        {
            // Create small version with an "X"
            var smallUserButton = new Button()
            {
                Content = $"{user.Username} X",
                Tag = user,
                Background = new SolidColorBrush(Microsoft.UI.Colors.Cyan),
                Foreground = new SolidColorBrush(Microsoft.UI.Colors.White),
            };
            smallUserButton.Click += (s, e) => RemoveUserFromSelectedList(user);

            // Add to the selected users panel
            SelectedUsersPanel.Children.Add(smallUserButton);
        }

        private void UserSearchBox_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            // Show the search results when the user taps inside the search box.
            UserSearchResults.Visibility = Visibility.Visible;
        }

        private void UserSearchResults_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            // Allow users to select a user from the list, without hiding the dropdown immediately.
            var selectedUser = (User)((ListBox)sender).SelectedItem;
            AddUserToSelectedList(selectedUser);
            // Optionally, you can hide the results after selection
            UserSearchResults.Visibility = Visibility.Collapsed;
        }

        // Remove user from the selected list (only small version)
        private void RemoveUserFromSelectedList(User user)
        {
            var smallUserButton = SelectedUsersPanel.Children
                .OfType<Button>()
                .FirstOrDefault(button => button.Tag == user);

            if (smallUserButton != null)
            {
                SelectedUsersPanel.Children.Remove(smallUserButton);
            }
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
                    image = await AppController.EncodeImageToBase64Async(file);
                }
            }
            catch (Exception ex)
            {
                ErrorTextBox.Text = $"Error uploading image: {ex.Message}";
            }
        }

        private void RemoveImageButton_Click(object sender, RoutedEventArgs e)
        {
            image = string.Empty;
        }
    }
}
