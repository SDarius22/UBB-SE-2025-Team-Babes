using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialApp.Entities;

namespace SocialApp.Services
{
    class CommentService
    {
        public CommentService() { }
        public void ValidateAdd(string content, long userId, long postId)
        {
            if (content == null || content.Length == 0)
            {
                throw new Exception("Comment content cannot be empty");
            }
            if (userId < 0)
            {
                throw new Exception("User ID cannot be less than 0");
            }
            if (postId < 0)
            {
                throw new Exception("Post ID cannot be less than 0");
            }
        }
        public void ValidateDelete(long commentId)
        {
            if (commentId < 0)
            {
                throw new Exception("Comment ID cannot be less than 0");
            }
        }
        public void ValidateUpdate(long commentId, string content)
        {
            if (commentId < 0)
            {
                throw new Exception("Comment ID cannot be less than 0");
            }
            if (content == null || content.Length == 0)
            {
                throw new Exception("Comment content cannot be empty");
            }
        }
        public List<Comment> GetAll()
        {
            return new List<Comment>();
        }
        public Comment GetById(int id)
        {
            return new Comment() { Id = id, Content = "Comment 1", UserId = 1, PostId = 1 };
        }
    }
}
