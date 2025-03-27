using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SocialApp.Enums;
using SocialApp.Repository;
using SocialApp.Services;
using System.Collections.Generic;
using System.Linq;

namespace SocialApp.Components
{
    public sealed partial class PostsFeed : UserControl
    {
        private int currentPage = 1;
        private const int postsPerPage = 5;
        private List<PostComponent> allPosts;
        private UserRepository userRepository;
        private UserService userService;
        private PostRepository postRepository;
        private PostService postService;
        private GroupRepository groupRepository;
        private long userId = 1; // Replace with actual userId

        public StackPanel PostsStackPanelPublic => PostsStackPanel;

        public PostsFeed()
        {
            this.InitializeComponent();

            userRepository = new UserRepository();
            userService = new UserService(userRepository);
            postRepository = new PostRepository();
            groupRepository = new GroupRepository();
            postService = new PostService(postRepository, userRepository, groupRepository);
            allPosts = new List<PostComponent>();

            LoadPosts();
            DisplayCurrentPage();
        }

        public void AddPost(PostComponent post)
        {
            allPosts.Add(post);
        }

        private void LoadPosts()
        {
            var posts = postService.GetHomeFeed(userId).ToList();
            foreach (var post in posts)
            {
                var postComponent = new PostComponent(post.Title, post.Visibility, post.UserId, post.Content, post.CreatedDate, post.Id);
                allPosts.Add(postComponent);
            }
        }

        public void DisplayCurrentPage()
        {
            PostsStackPanel.Children.Clear();
            int startIndex = (currentPage - 1) * postsPerPage;
            int endIndex = startIndex + postsPerPage;
            for (int i = startIndex; i < endIndex && i < allPosts.Count; i++)
            {
                PostsStackPanel.Children.Add(allPosts[i]);
            }
        }

        public void ClearPosts()
        {
            allPosts = new List<PostComponent>();
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
