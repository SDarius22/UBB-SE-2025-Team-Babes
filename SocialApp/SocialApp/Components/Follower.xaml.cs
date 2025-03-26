using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace SocialApp.Components
{
    public sealed partial class Follower : UserControl
    {
        public Follower(string username)
        {
            this.InitializeComponent();
            Name.Text = username;
            // Set Button.Content to "Follow" or "Unfollow" based on logic if needed
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Implement follow/unfollow logic here if needed
            Button.Content = Button.Content.ToString() == "Follow" ? "Unfollow" : "Follow";
        }
    }
}