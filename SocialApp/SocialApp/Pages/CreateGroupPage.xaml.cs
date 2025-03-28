using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SocialApp.Services;
using SocialApp.Repository;
using SocialApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Media;

namespace SocialApp.Pages
{
    public sealed partial class CreateGroupPage : Page
    {
        private AppController _controller;
        private GroupService _groupService;
        private UserService _userService;

        public CreateGroupPage()
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
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is AppController controller)
            {
                _controller = controller;
                TopBar.SetControllerAndFrame(_controller, Frame);
            }
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

                var newGroup = new Group
                {
                    Name = GroupNameInput.Text.Trim(),
                    Description = string.IsNullOrWhiteSpace(GroupDescriptionInput.Text) ? null : GroupDescriptionInput.Text.Trim(),
                    AdminId = _controller.CurrentUser.Id,
                    Image = null // Image handling can be implemented later
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

        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            // Add image logic here
        }

        private void RemoveImageButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove image logic here
        }
    }
}