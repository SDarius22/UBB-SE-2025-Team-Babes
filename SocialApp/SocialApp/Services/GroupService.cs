using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialApp.Entities;

namespace SocialApp.Services
{
    class GroupService
    {
        public GroupService() { }
        public void ValidateAdd(string name, long adminId) 
        {
            if (name == null || name.Length == 0)
            {
                throw new Exception("Group name cannot be empty");
            }
            if (adminId < 0)
            {
                throw new Exception("Admin ID cannot be less than 0");
            }
            Group group = new Group() { Name = name, AdminId = adminId };


        }
        public void ValidateDelete(long groupId)
        {
            if (groupId < 0)
            {
                throw new Exception("Group ID cannot be less than 0");
            }


        }
        public void ValidateUpdate(Group group)
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
