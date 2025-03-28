using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SocialApp.Enums;
using SocialApp.Repository;
using System.Linq;
using SocialApp.Services;

namespace SocialApp.Components
{
    public sealed partial class PostComponent : UserControl
    {
        private string title;
        private PostVisibility visibility;
        private long userId;
        private string content;
        private DateTime createdDate;
        private long postId;

        private ReactionService reactionService;
        private CommentService commentService;

        public DateTime PostCreationTime { get; set; }

        public string TimeSincePost
        {
            get
            {
                var timeSpan = DateTime.Now - PostCreationTime;
                if (timeSpan.TotalDays >= 1)
                {
                    return $"{(int)timeSpan.TotalDays} days ago";
                }
                else if (timeSpan.TotalHours >= 1)
                {
                    return $"{(int)timeSpan.TotalHours} hours ago";
                }
                else
                {
                    return $"{(int)timeSpan.TotalMinutes} minutes ago";
                }
            }
        }

        public PostVisibility PostVisibility
        {
            get => visibility;
            set
            {
                visibility = value;
                VisibilityText.Text = visibility.ToString(); // Update the UI element
            }
        }

        public PostComponent()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.reactionService = new ReactionService(new ReactionRepository());
            this.commentService = new CommentService(new CommentRepository(), new PostRepository(), new UserRepository());
        }

        public PostComponent(string title, PostVisibility visibility, long userId, string content, DateTime createdDate, long postId)
        {
            this.title = title;
            this.DataContext = this;
            this.InitializeComponent();
            this.PostVisibility = visibility; // Use the property to set visibility
            this.userId = userId;
            this.content = content;
            this.createdDate = createdDate;
            this.postId = postId;

            Title.Text = title;
            Content.Text = content;
            TimeSince.Text = createdDate.ToString();

            this.reactionService = new ReactionService(new ReactionRepository());
            this.commentService = new CommentService(new CommentRepository(), new PostRepository(), new UserRepository());
            LoadReactionCounts();
        }

        private void LoadReactionCounts()
        {
            var reactions = reactionService.GetReactionsForPost(postId);
            LikeCount.Text = reactions.Count(r => r.Type == ReactionType.Like).ToString();
            LoveCount.Text = reactions.Count(r => r.Type == ReactionType.Love).ToString();
            LaughCount.Text = reactions.Count(r => r.Type == ReactionType.Laugh).ToString();
            AngryCount.Text = reactions.Count(r => r.Type == ReactionType.Anger).ToString();
        }

        private void LoadComments()
        {
            var comments = commentService.GetCommentForPost(postId);
            CommentsListView.ItemsSource = comments;
        }
        private void OnLikeButtonClick(object sender, RoutedEventArgs e)
        {
            reactionService.ValidateAdd(userId, postId, ReactionType.Like);
            LoadReactionCounts();
        }

        private void OnLoveButtonClick(object sender, RoutedEventArgs e)
        {
            reactionService.ValidateAdd(userId, postId, ReactionType.Love);
            LoadReactionCounts();
        }

        private void OnLaughButtonClick(object sender, RoutedEventArgs e)
        {
            reactionService.ValidateAdd(userId, postId, ReactionType.Laugh);
            LoadReactionCounts();
        }

        private void OnAngryButtonClick(object sender, RoutedEventArgs e)
        {
            reactionService.ValidateAdd(userId, postId, ReactionType.Anger);
            LoadReactionCounts();
        }

        private void OnCommentButtonClick(object sender, RoutedEventArgs e)
        {
            LoadComments();
            CommentSection.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
        }

        private void OnSubmitCommentButtonClick(object sender, RoutedEventArgs e)
        {
            // Handle comment submission
            string commentText = CommentTextBox.Text;
            if (!string.IsNullOrEmpty(commentText))
            {
                // Save the comment using CommentService
                commentService.ValidateAdd(commentText, userId, postId);

                // Clear the TextBox and hide the comment section
                CommentTextBox.Text = string.Empty;
                CommentSection.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
            }
        }
    }
}
