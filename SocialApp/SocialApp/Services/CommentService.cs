using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialApp.Entities;
using SocialApp.Repository;

namespace SocialApp.Services
{
    class CommentService
    {
        CommentRepository CommentRepository;
        PostRepository PostRepository;
        UserRepository UserRepository;
        public CommentService(CommentRepository cr, PostRepository pr, UserRepository userRepository)
        {
            this.CommentRepository = cr;
            this.PostRepository = pr;
            this.UserRepository = userRepository;
        }
        public Comment ValidateAdd(string content, long userId, long postId)
        {
            if (content == null || content.Length == 0)
            {
                throw new Exception("Comment content cannot be empty");
            }
            if (UserRepository.GetById(userId) == null)
            {
                throw new Exception("User does not exist");
            }
            if (PostRepository.GetById(postId) == null)
            {
                throw new Exception("Post does not exist");
            }
            Comment comment = new Comment() { Content = content, UserId = userId, PostId = postId, CreatedDate = DateTime.Now };
            CommentRepository.Save(comment);
            return comment;
        }
        public void ValidateDelete(long commentId)
        {
            if (CommentRepository.GetById(commentId) == null)
            {
                throw new Exception("Comment does not exist");
            }
            CommentRepository.DeleteById(commentId);

        }
        public void ValidateUpdate(long commentId, string content)
        {
            if (CommentRepository.GetById(commentId) == null)
            {
                throw new Exception("Comment does not exist");
            }
            if (content == null || content.Length == 0)
            {
                throw new Exception("Comment content cannot be empty");
            }
            CommentRepository.UpdateById(commentId, content); 
        }
        public List<Comment> GetAll()
        {
            return CommentRepository.GetAll();
        }
        public Comment GetById(int id)
        {
            return CommentRepository.GetById(id);
        }

        public List<Comment> GetCommentForPost(long postId)
        {
            return CommentRepository.GetCommentsForPost(postId);
        }
    }
}
