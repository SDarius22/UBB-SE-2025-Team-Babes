using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace SocialApp.Components
{
    public sealed partial class PostComponent : UserControl
    {
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
            PostCreationTime = DateTime.Now; // For demonstration purposes, set the post creation time to now
        }
    }
}
