using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialApp.Entities;
using SocialApp.Enums;
using SocialApp.Repository;

namespace SocialApp.Services
{
    class ReactionService
    {
        ReactionRepository ReactionRepository;

        public ReactionService(ReactionRepository ReactionRepository)
        {
            this.ReactionRepository = ReactionRepository;
        }

        public void ValidateAdd(long userId, long postId, ReactionType type)
        {
            if (ReactionRepository.GetByUserAndPost(userId, postId) != null)
            {
                ReactionRepository.UpdateByUserAndPost(userId, postId, type);
            }
            ReactionRepository.Save(new Reaction() { UserId = userId, PostId = postId, Type=type });
        }

        public void ValidateDelete(long userId, long postId)
        {
            Reaction reaction = ReactionRepository.GetByUserAndPost(userId, postId);
            if (reaction == null)
            {
                throw new Exception("Reaction does not exist");
            }
            ReactionRepository.DeleteByUserAndPost(userId, postId);
        }
        public List<Reaction> GetAll()
        {
            return ReactionRepository.GetAll();
        }

        public List<Reaction> GetByPost(long postId)
        {
            return ReactionRepository.GetByPost(postId);
        }
    }
}
