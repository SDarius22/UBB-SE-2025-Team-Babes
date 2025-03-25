using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace SocialApp.Components
{
    public sealed partial class PostsFeed : UserControl
    {
        private int currentPage = 1;
        private const int postsPerPage = 5;
        private List<PostComponent> allPosts;

        public PostsFeed()
        {
            this.InitializeComponent();
            LoadPosts();
            DisplayCurrentPage();
        }

        private void LoadPosts()
        {
            // Load all posts (this is just a placeholder, replace with actual data loading logic)
            allPosts = new List<PostComponent>();
            for (int i = 1; i <= 20; i++)
            {
                allPosts.Add(new PostComponent { Margin = new Thickness(0, 0, 0, 10) });
            }
        }

        private void DisplayCurrentPage()
        {
            PostsStackPanel.Children.Clear();
            int startIndex = (currentPage - 1) * postsPerPage;
            int endIndex = startIndex + postsPerPage;
            for (int i = startIndex; i < endIndex && i < allPosts.Count; i++)
            {
                PostsStackPanel.Children.Add(allPosts[i]);
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
            if (currentPage * postsPerPage < allPosts.Count)
            {
                currentPage++;
                DisplayCurrentPage();
            }
        }
    }
}
