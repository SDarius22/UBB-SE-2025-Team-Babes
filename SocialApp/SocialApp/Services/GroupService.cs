using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialApp.Entities;
using SocialApp.Repository;

namespace SocialApp.Services
{
    class GroupService
    {
        GroupRepository GroupRepository;
        UserRepository UserRepository;
        public GroupService(GroupRepository groupRepository, UserRepository userRepository)
        {
            this.GroupRepository = groupRepository;
            this.UserRepository = userRepository;
        }

        public Group ValidateAdd(string name, string desc, long adminId) 
        {
            if (name == null || name.Length == 0)
            {
                throw new Exception("Group name cannot be empty");
            }
            if (UserRepository.GetById(adminId) == null)
            {
               throw new Exception("User does not exist");
            }
            Group group = new Group() { Name = name, AdminId = adminId };
            GroupRepository.Save(group);
            return group;
        }
        public void ValidateDelete(long groupId)
        {
            if (GroupRepository.GetById(groupId) == null)
            {
                throw new Exception("Group does not exist");
            }
            GroupRepository.DeleteById(groupId);
        }

        public void ValidateUpdate()
        {
            
        }
        public List<Group> GetAll()
        {
            return new List<Group>();
        }
        public Group GetById(int id)
        {
            return new Group() { Id = id, Name = "Group 1", AdminId = 1 };
        }
    }
}
