using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialApp.Entities;
using SocialApp.Repository;

namespace SocialApp.Services
{
    class UserService
    {
        UserRepository userRepository;

        public UserService(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void ValidateAdd(string username, string email, string password)
        {
            if (username == null || username.Length == 0)
            {
                throw new Exception("Username cannot be empty");
            }
            if (email == null || email.Length == 0)
            {
                throw new Exception("Email cannot be empty");
            }
            if (password == null || password.Length == 0)
            {
                throw new Exception("Password cannot be empty");
            }
            userRepository.Save(new User() { Username = username, Email = email, PasswordHash = password });
        }

        public void ValidateDelete(long id)
        {
            if (userRepository.GetById(id) == null)
            {
                throw new Exception("User does not exist");
            }
            userRepository.DeleteById(id);
        }

        public void ValidateUpdate(long id, string username, string email, string password)
        {
            if (userRepository.GetById(id) == null)
            {
                throw new Exception("User does not exist");
            }
            userRepository.UpdateById(id, username, email, password);
        }

        public List<User> GetAll()
        {
            return userRepository.GetAll();
        }

        public User GetById(long id)
        {
            return userRepository.GetById(id);
        }

        public List<User> GetUserFollowers(long id)
        {
            return userRepository.GetUserFollowers(id);
        }
    }
}
