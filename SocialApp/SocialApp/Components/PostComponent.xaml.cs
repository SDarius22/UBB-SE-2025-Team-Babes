using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SocialApp.Enums;

namespace SocialApp.Components
{
    public sealed partial class PostComponent : UserControl
    {
        private string title;
        private PostVisibility visibility;
        private long userId;
        private string content;
        private DateTime createdDate;

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

        public PostComponent()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public PostComponent(string title, PostVisibility visibility, long userId, string content, DateTime createdDate)
        {
            this.title = title;
            this.DataContext = this;
            this.InitializeComponent();
            this.visibility = visibility;
            this.userId = userId;
            this.content = content;
            this.createdDate = createdDate;

            Title.Text = title;
            Content.Text = content;
            TimeSince.Text = createdDate.ToString();
        }
    }
}
