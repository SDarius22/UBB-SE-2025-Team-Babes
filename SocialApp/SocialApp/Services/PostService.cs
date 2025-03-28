using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SocialApp.Entities;
using SocialApp.Enums;
using SocialApp.Repository;

namespace SocialApp.Services
{
    class PostService
    {
        PostRepository postRepository;
        UserRepository userRepository;
        GroupRepository groupRepository;
        public PostService(PostRepository postRepository, UserRepository userRepository, GroupRepository gr)
        {
            this.postRepository = postRepository;
            this.userRepository = userRepository;
            this.groupRepository = gr;
        }

        public Post ValidateAdd(string title, string? content, long userId, long groupId, PostVisibility postVisibility, PostTag postTag)
        {
            if (title == null || title.Length == 0)
            {
                throw new Exception("Post title cannot be empty");
            }
            if (userRepository.GetById(userId) == null)
            {
                throw new Exception("User does not exist");
            }
            if (groupId != 0)
            {
                if (groupRepository.GetById(groupId) == null)
                {
                    throw new Exception("Group does not exist");
                }
            }
            Post post = new Post() { Title = title, Description = content, UserId = userId, GroupId = groupId, Visibility = postVisibility, Tag = postTag, CreatedDate = DateTime.Now };
            postRepository.Save(post);
            return post;
        }

        public void ValidateDelete(long id)
        {
            if (postRepository.GetById(id) == null)
            {
                throw new Exception("Post does not exist");
            }
            postRepository.DeleteById(id);
        }

        public void ValidateUpdate(long id, string title, string description, PostVisibility visibility, PostTag tag)
        {
            if (postRepository.GetById(id) == null)
            {
                throw new Exception("Post does not exist");
            }
            postRepository.UpdateById(id, title, description, visibility, tag);
        }

        public List<Post> GetAll()
        {
            return postRepository.GetAll();
        }

        public Post GetById(long id)
        {
            return postRepository.GetById(id);
        }

        public List<Post> GetByUserId(long userId)
        {
            return postRepository.GetByUser(userId);
        }

        public List<Post> GetByGroupId(long groupId)
        {
            return postRepository.GetByGroup(groupId);
        }

        public List<Post> GetHomeFeed(long userId)
        {
            return postRepository.GetHomeFeed(userId);
        }

        public List<Post> GetGroupsFeed(long userId)
        {
            return postRepository.GetGroupsFeed(userId);
        }
            


    }
}
