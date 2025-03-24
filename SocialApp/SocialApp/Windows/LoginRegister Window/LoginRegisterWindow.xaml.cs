using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SocialApp.Windows.LoginRegister_Window
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginRegisterWindow : Window
    {
        public LoginRegisterWindow()
        {
            this.InitializeComponent();
            this.InitialFlow();
        }

        private void InitialFlow()
        {
            this.SetInitialVisibilities();
            this.SetInitialContent();
        }

        private void SetInitialVisibilities()
        {
            EmailTextbox.Visibility = Visibility.Visible;
            PasswordTextbox.Visibility = Visibility.Collapsed;
            ConfirmPasswordTextbox.Visibility = Visibility.Collapsed;
            UploadedImage.Visibility = Visibility.Collapsed;
            UploadImgButton.Visibility = Visibility.Collapsed;
            RemoveImgButton.Visibility = Visibility.Collapsed;
            CheckBox.Visibility = Visibility.Collapsed;
            ContinueButton.Visibility = Visibility.Visible;
        }

        private void SetInitialContent()
        {
            PageName.Text = "Login/Register";
            ContinueButton.Content = "Continue";
        }

        public void ContinueClick(object sender, RoutedEventArgs e)
        {
            if (IsValidEmail(EmailTextbox.Text))
            {
                if (IsRegisteredEmail(EmailTextbox.Text))
                {
                    LoginFlow();
                }
                else
                {
                    RegisterFlow();
                }
            }
            else
            {
                ErrorTextbox.Text = "Wrong email format.";
            }
        }

        private bool IsValidEmail(String email)
        {
            return false;
        }

        private bool IsRegisteredEmail(String email)
        {
            return true;
        }

        private void LoginFlow()
        {

        }
        private void RegisterFlow()
        {

        }
    }
}
