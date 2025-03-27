using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using SocialApp.Services;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace SocialApp.Pages
{
    public sealed partial class LoginRegisterPage : Page
    {
        private const Visibility collapsed = Visibility.Collapsed;
        private const Visibility visible = Visibility.Visible;
        private AppController controller;
        private string image;

        public LoginRegisterPage()
        {
            this.InitializeComponent();
            this.InitialFlow();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.controller = App.Services.GetService<AppController>();
        }

        private void InitialFlow()
        {
            SetInitialVisibilities();
            SetInitialContent();
            SetInitialHandlers();
        }

        private void SetInitialVisibilities()
        {
            EmailTextbox.Visibility = visible;
            UsernameTextbox.Visibility = collapsed;
            PasswordTextbox.Visibility = collapsed;
            ConfirmPasswordTextbox.Visibility = collapsed;
            UploadedImage.Visibility = collapsed;
            UploadImgButton.Visibility = collapsed;
            RemoveImgButton.Visibility = collapsed;
            CheckBox.Visibility = collapsed;
            ContinueButton.Visibility = visible;
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
            if (controller.EmailExists(EmailTextbox.Text))
            {
                LoginFlow();
            }
            else
            {
                RegisterFlow();
            }
        }

        private void LoginFlow()
        {
            SetLoginVisibilities();
            SetLoginContent();
            SetLoginHandlers();
        }

        private void SetLoginVisibilities()
        {
            PasswordTextbox.Visibility = visible;
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
            if (!controller.Login(EmailTextbox.Text, PasswordTextbox.Password)) // Use Password property
            {
                ErrorTextbox.Visibility = visible;
                ErrorTextbox.Text = "Incorrect password.";
                PasswordTextbox.Password = ""; // Clear password
            }
            else
            {
                Frame.Navigate(typeof(HomeScreen), controller);
            }
        }

        private void RegisterFlow()
        {
            SetRegisterVisibilities();
            SetRegisterContent();
            SetRegisterHandlers();
        }

        private void SetRegisterVisibilities()
        {
            PasswordTextbox.Visibility = visible;
            UsernameTextbox.Visibility = visible;
            ConfirmPasswordTextbox.Visibility = visible;
            UploadedImage.Visibility = visible;
            UploadImgButton.Visibility = visible;
            RemoveImgButton.Visibility = visible;
            CheckBox.Visibility = visible;
        }

        private void SetRegisterContent()
        {
            PageName.Text = "Register";
            ContinueButton.Content = "Register";
            ErrorTextbox.Text = "";
            UploadedImage.Child = new Image
            {
                Source = new BitmapImage(new Uri("ms-appx:///Assets/User.png"))
            };
            image = string.Empty;
        }

        private void SetRegisterHandlers()
        {
            ContinueButton.Click -= ContinueClick;
            ContinueButton.Click += RegisterClick;
        }

        private async void UploadImage(object sender, RoutedEventArgs e)
        {
            try
            {
                var picker = new FileOpenPicker
                {
                    ViewMode = PickerViewMode.Thumbnail,
                    SuggestedStartLocation = PickerLocationId.PicturesLibrary
                };
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");
                picker.FileTypeFilter.Add(".png");

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.CurrentWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

                StorageFile file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    image = await AppController.EncodeImageToBase64Async(file);
                    var bitmapImage = new BitmapImage();
                    using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                    {
                        await bitmapImage.SetSourceAsync(stream);
                    }
                    UploadedImage.Child = new Image { Source = bitmapImage };
                }
            }
            catch (Exception ex)
            {
                ErrorTextbox.Text = $"Error uploading image: {ex.Message}";
            }
        }

        private void RemoveImage(object sender, RoutedEventArgs e)
        {
            image = string.Empty;
            UploadedImage.Child = new Image
            {
                Source = new BitmapImage(new Uri("ms-appx:///Assets/User.png"))
            };
        }

        private void RegisterClick(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordsMatch(PasswordTextbox.Password, ConfirmPasswordTextbox.Password); // Use Password property
                AreTermAccepted();
                Register();
            }
            catch (Exception ex)
            {
                ErrorTextbox.Text = ex.Message;
            }
        }

        private void PasswordsMatch(string password, string confirmedPassword)
        {
            if (password != confirmedPassword)
            {
                throw new Exception("Passwords must match");
            }
        }

        private void AreTermAccepted()
        {
            if (CheckBox.IsChecked == null || CheckBox.IsChecked == false)
            {
                throw new Exception("You must accept the terms and conditions!");
            }
        }

        private void Register()
        {
            try
            {
                controller.Register(UsernameTextbox.Text, EmailTextbox.Text, PasswordTextbox.Password, image); // Use Password property
                Frame.Navigate(typeof(HomeScreen), controller);
            }
            catch (Exception ex)
            {
                ErrorTextbox.Text = ex.Message;
            }
        }
    }
}