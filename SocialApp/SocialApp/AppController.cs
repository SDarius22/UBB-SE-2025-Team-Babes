using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using SocialApp.Entities;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.Storage;
using SocialApp.Repository;

namespace SocialApp
{
    public sealed class AppController
    {
        private static readonly Lazy<AppController> instance = new(() => new AppController());

        public static AppController Instance => instance.Value;

        public User? CurrentUser { get; private set; } = null;

        private AppController() { }

        public bool Login(string email, string password)
        {
            UserRepository userRepository = new UserRepository();
            User user = userRepository.GetByEmail(email);
            if (user != null && user.PasswordHash == password)
            {
                CurrentUser = user;
                return true;
            }
            return false;
        }

        public void Logout()
        {
            CurrentUser = null;
        }


        public static async Task<string> EncodeImageToBase64Async(StorageFile imageFile)
        {
            using (IRandomAccessStream stream = await imageFile.OpenAsync(FileAccessMode.Read))
            {
                var decoder = await BitmapDecoder.CreateAsync(stream);
                var pixelData = await decoder.GetPixelDataAsync();
                byte[] bytes = pixelData.DetachPixelData();
                return Convert.ToBase64String(bytes);
            }
        }

        public static async Task<SoftwareBitmap> DecodeBase64ToImageAsync(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(bytes.AsBuffer());
                stream.Seek(0);
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                return await decoder.GetSoftwareBitmapAsync();
            }
        }

    }
}
