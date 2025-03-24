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
            SetInitialVisibilities();
            SetInitialContent();
            SetInitialHandlers();
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

        private void SetInitialHandlers()
        {
            ContinueButton.Click += ContinueClick;
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
                ErrorTextbox.Text = "Provide a valid email.";
            }
        }

        private bool IsValidEmail(String email)
        {
            return !email.Equals(String.Empty);
        }

        private bool IsRegisteredEmail(String email)
        {
            return false;
        }

        private void LoginFlow()
        {
            SetLoginVisibilities();
            SetLoginContent();
            SetLoginHandlers();
        }

        private void SetLoginVisibilities()
        {
            PasswordTextbox.Visibility = Visibility.Visible;
        }

        private void SetLoginContent()
        {
            PageName.Text = "Login";
            ContinueButton.Content = "Login";
            ErrorTextbox.Text = "";
        }

        private void SetLoginHandlers()
        {
            ContinueButton.Click -= ContinueClick;
            ContinueButton.Click += LoginClick;
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            if (IsCorrectPassword(EmailTextbox.Text, PasswordTextbox.Text))
            {
                Login();
            }
            else
            {
                ErrorTextbox.Visibility = Visibility.Visible;
                ErrorTextbox.Text = "Incorrect password.";
                PasswordTextbox.Text = "";
            }
        }

        private bool IsCorrectPassword(String username, String password)
        {
            return true;
        }

        private void Login()
        {
            
        }

        private void RegisterFlow()
        {
            SetRegisterVisibilities();
            SetRegisterContent();
            SetRegisterHandlers();
        }
        private void SetRegisterVisibilities()
        {
            PasswordTextbox.Visibility = Visibility.Visible;
            UploadedImage.Visibility = Visibility.Visible;
            UploadImgButton.Visibility = Visibility.Visible;
            RemoveImgButton.Visibility = Visibility.Visible;
            CheckBox.Visibility = Visibility.Visible;
        }

        private void SetRegisterContent()
        {
            PageName.Text = "Register";
            ContinueButton.Content = "Register";
            ErrorTextbox.Text = "";
        }

        private void SetRegisterHandlers()
        {
            ContinueButton.Click -= ContinueClick;
            ContinueButton.Click += RegisterClick;
        }

        private void RegisterClick(object sender, RoutedEventArgs e)
        {
            Register();
        }

        private void Register()
        {

        }
    }
}
