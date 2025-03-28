using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System.Collections.Generic;
using SocialApp.Entities;
using SocialApp.Services;
using SocialApp.Pages;
using SocialApp.Windows;
using SocialApp.Repository; // Add this for GroupRepository and UserRepository

namespace SocialApp.Components
{
    public sealed partial class GroupsDrawer : UserControl
    {
        private GroupService _groupService;
        private AppController controller;
        private Frame frame;

        public GroupsDrawer()
        {
            this.InitializeComponent();
            var groupRepository = new GroupRepository();
            var userRepository = new UserRepository();
            _groupService = new GroupService(groupRepository, userRepository);
            LoadGroups();
        }

        public void SetControllerAndFrame(AppController controller, Frame frame)
        {
            this.controller = controller;
            this.frame = frame;
        }

        private void LoadGroups()
        {
            GroupsList.Children.Clear(); // Clear old items

            var groups = _groupService.GetAll(); // Fetch from DB

            foreach (var group in groups)
            {
                var groupItem = new StackPanel { Orientation = Orientation.Vertical, Margin = new Thickness(0, 0, 0, 10) };

                var groupHeader = new StackPanel { Orientation = Orientation.Horizontal };
                groupHeader.Children.Add(new TextBlock { Text = "★", Foreground = new SolidColorBrush(Colors.Gold), FontSize = 18, VerticalAlignment = VerticalAlignment.Center });
                groupHeader.Children.Add(new TextBlock { Text = group.Name, FontSize = 18, Foreground = new SolidColorBrush(Colors.White), Margin = new Thickness(5, 0, 0, 0) });

                var groupDescription = new TextBlock { Text = group.Description, FontSize = 14, Foreground = new SolidColorBrush(Colors.Gray), Margin = new Thickness(23, 0, 0, 0) };

                groupItem.Children.Add(groupHeader);
                groupItem.Children.Add(groupDescription);

                GroupsList.Children.Add(groupItem);
            }
        }

    }


    public class Group
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
