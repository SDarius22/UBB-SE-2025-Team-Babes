using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml.Controls;
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

        public Group ValidateAdd(string name, string desc, string image, long adminId) 
        {
            if (name == null || name.Length == 0)
            {
                throw new Exception("Group name cannot be empty");
            }
            if (UserRepository.GetById(adminId) == null)
            {
               throw new Exception("User does not exist");
            }
            Group group = new Group() { Name = name, AdminId = adminId, Image=image, Description=desc };
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

        public void ValidateUpdate(long id, string name, string desc, string image, long adminId)
        {
            if (GroupRepository.GetById(id) == null)
            {
                throw new Exception("Group does not exist");
            }
            if (UserRepository.GetById(adminId) == null)
            {
                throw new Exception("User does not exist");
            }
            if (name == null || name.Length == 0)
            {
                throw new Exception("Group name cannot be empty");
            }
            GroupRepository.UpdateById(id, name, image, desc, adminId);
        }
        public List<Group> GetAll()
        {
            return GroupRepository.GetAll();
        }
        public Group GetById(long id)
        {
            return GroupRepository.GetById(id);
        }

        public List<User> GetUsersFromGroup(long groupId)
        {
            return GroupRepository.GetUsersFromGroup(groupId);
        }

        public List<Group> GetGroupsForUser(long userId)
        {
            return GroupRepository.GetGroupsForUser(userId);
        }
    }
}
