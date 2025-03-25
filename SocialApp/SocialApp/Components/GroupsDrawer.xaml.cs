using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media; // Add this for SolidColorBrush
using Microsoft.UI; // Add this for Colors
using System.Collections.Generic;
namespace SocialApp.Components
{
    public sealed partial class GroupsDrawer : UserControl
    {
        public GroupsDrawer()
        {
            this.InitializeComponent();
            LoadGroups();
        }

        private void LoadGroups()
        {
            // Example data, replace with actual data loading logic
            var groups = new List<Group>
                {
                    new Group { Name = "Group 1", Description = "Description for Group 1" },
                    new Group { Name = "Group 2", Description = "Description for Group 2" },
                    new Group { Name = "Group 3", Description = "Description for Group 3" }
                };

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
