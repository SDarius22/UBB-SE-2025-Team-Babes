using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SocialApp.Enums;
using SocialApp.Repository;
using SocialApp.Services;
using System.Collections.Generic;
using System.Linq;

namespace SocialApp.Components
{
    public sealed partial class GroupsFeed : UserControl
    {
        private int currentPage = 1;
        private const int itemsPerPage = 5;
        private List<PostComponent> allItems;
        private UserRepository userRepository;
        private UserService userService;
        private PostRepository postRepository;
        private PostService postService;
        private GroupRepository groupRepository;
        private GroupService groupService;
        private long groupId = 1; // Replace with actual groupId

        public GroupsFeed()
        {
            this.InitializeComponent();

            userRepository = new UserRepository();
            userService = new UserService(userRepository);
            postRepository = new PostRepository();
            groupRepository = new GroupRepository();
            postService = new PostService(postRepository, userRepository, groupRepository);
            groupService = new GroupService(groupRepository, userRepository);
            allItems = new List<PostComponent>();

            LoadItems();
            DisplayCurrentPage();
        }

        private void LoadItems()
        {
            var posts = postService.GetByGroupId(groupId).Where(p => p.Visibility == PostVisibility.Groups).ToList();
            foreach (var post in posts)
            {
                var postComponent = new PostComponent(post.Title, post.Visibility, post.UserId, post.Content, post.CreatedDate, post.Id);
                allItems.Add(postComponent);
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
