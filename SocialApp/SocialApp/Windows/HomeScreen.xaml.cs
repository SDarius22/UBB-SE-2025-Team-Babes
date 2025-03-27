using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SocialApp.Pages;
using SocialApp.Windows;
using Microsoft.UI.Xaml.Navigation;
using SocialApp.Pages;
using SocialApp.Windows;
using System;

namespace SocialApp
{
    public sealed partial class HomeScreen : Page
    {
        private AppController controller;

        public HomeScreen()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is AppController controller)
            {
                this.controller = controller;
                TopBar.SetControllerAndFrame(controller, this.Frame);
            }
        }
    }
}
