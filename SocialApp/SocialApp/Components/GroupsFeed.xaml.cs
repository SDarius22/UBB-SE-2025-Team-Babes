using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace SocialApp.Components
{
    public sealed partial class GroupsFeed : UserControl
    {
        private int currentPage = 1;
        private const int itemsPerPage = 5;
        private List<PostComponent> allItems;

        public GroupsFeed()
        {
            this.InitializeComponent();
            LoadItems();
            DisplayCurrentPage();
        }

        private void LoadItems()
        {
            // Load all items (this is just a placeholder, replace with actual data loading logic)
            allItems = new List<PostComponent>();
            for (int i = 1; i <= 20; i++)
            {
                allItems.Add(new PostComponent { Margin = new Thickness(0, 0, 0, 10) });
            }
        }

        private void DisplayCurrentPage()
        {
            GroupsStackPanel.Children.Clear();
            int startIndex = (currentPage - 1) * itemsPerPage;
            int endIndex = startIndex + itemsPerPage;
            for (int i = startIndex; i < endIndex && i < allItems.Count; i++)
            {
                GroupsStackPanel.Children.Add(allItems[i]);
            }
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                DisplayCurrentPage();
            }
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage * itemsPerPage < allItems.Count)
            {
                currentPage++;
                DisplayCurrentPage();
            }
        }
    }
}
