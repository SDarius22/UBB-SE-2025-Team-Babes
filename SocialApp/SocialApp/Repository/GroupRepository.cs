using Microsoft.Data.SqlClient;
using SocialApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Group = SocialApp.Entities.Group;

namespace SocialApp.Repository
{
    public class GroupRepository
    {
        private string loginString = "Data Source=ATHOS;" +
            "Initial Catalog=ISSDB;" +
            "Integrated Security=True;" +
            "TrustServerCertificate=True";
        private SqlConnection connection;

        public GroupRepository()
        {
            this.loginString = loginString;
            this.connection = new SqlConnection(loginString);
        }

        public List<Group> GetAll()
        {
            List<Group> ans = new List<Group>();
            connection.Open();

            SqlCommand selectCommand = new SqlCommand("SELECT * FROM Groups", connection);
            SqlDataReader reader = selectCommand.ExecuteReader();
            while (reader.Read())
            {
                Group group = new Group
                {
                    Id = reader.GetInt64(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    AdminId = reader.GetInt64(reader.GetOrdinal("AdminId")),
                    Image = reader.GetString(reader.GetOrdinal("Image")),
                    Description = reader.GetString(reader.GetOrdinal("Description"))
                };

                ans.Add(group);
            }
            reader.Close();
            connection.Close();

            return ans;
        }

        public List<User> GetUsersFromGroup(long id)
        {
            connection.Open();
            List<User> ans = new List<User>();
            SqlCommand selectCommand = new SqlCommand(
                "SELECT * FROM Users WHERE Id IN (SELECT UserId FROM GroupUsers WHERE GroupId = @Id)",
                connection
            );
            selectCommand.Parameters.AddWithValue("@Id", id);

            SqlDataReader reader = selectCommand.ExecuteReader();
            while (reader.Read())
            {
                User user = new User
                {
                    Id = reader.GetInt64(reader.GetOrdinal("Id")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    Image = reader.GetString(reader.GetOrdinal("Image"))
                };
                ans.Add(user);
            }
            reader.Close();
            connection.Close();
            return ans;

        }


        public List<Group> GetGroupsForUser(long userId)
        {
            connection.Open();
            List<Group> ans = new List<Group>();
            SqlCommand selectCommand = new SqlCommand(
                "SELECT * FROM Groups WHERE Id IN (SELECT GroupId FROM GroupUsers WHERE UserId = @UserId)",
                connection
            );
            selectCommand.Parameters.AddWithValue("@UserId", userId);
            SqlDataReader reader = selectCommand.ExecuteReader();

            while (reader.Read())
            {
                Group group = new Group
                {
                    Id = reader.GetInt64(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    AdminId = reader.GetInt64(reader.GetOrdinal("AdminId")),
                    Image = reader.GetString(reader.GetOrdinal("Image")),
                    Description = reader.GetString(reader.GetOrdinal("Description"))
                };
                ans.Add(group);
            }
            reader.Close();
            connection.Close();
            return ans;
        }

        public void DeleteById(long id)
        {
            connection.Open();

            SqlCommand deleteCommand = new SqlCommand("DELETE FROM Groups WHERE Id = @Id", connection);
            string queryParam = id.ToString();
            deleteCommand.Parameters.AddWithValue("@Id", queryParam);
            deleteCommand.ExecuteNonQuery();

            connection.Close();
        }

        public Group GetById(long id)
        {
            connection.Open();
            Group group = null;

            SqlCommand selectCommand = new SqlCommand("SELECT * FROM Groups WHERE Id = @Id", connection);
            selectCommand.Parameters.AddWithValue("@Id", id);

            SqlDataReader reader = selectCommand.ExecuteReader();
            if (reader.Read())
            {
                group = new Group
                {
                    Id = reader.GetInt64(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    AdminId = reader.GetInt64(reader.GetOrdinal("AdminId")),
                    Image = reader.GetString(reader.GetOrdinal("Image")),
                    Description = reader.GetString(reader.GetOrdinal("Description"))
                };
            }

            reader.Close();
            connection.Close();
            return group;
        }

        public void Save(Group entity)
        {
            connection.Open();

            SqlCommand insertCommand = new SqlCommand(
                "INSERT INTO Groups (Name, AdminId, Image, Description) VALUES (@Name, @AdminId, @Image, @Description)",
                connection
            );
            insertCommand.Parameters.AddWithValue("@Name", entity.Name);
            insertCommand.Parameters.AddWithValue("@AdminId", entity.AdminId);
            insertCommand.Parameters.AddWithValue("@Image", entity.Image);
            insertCommand.Parameters.AddWithValue("@Description", entity.Description);
            insertCommand.ExecuteNonQuery();

            connection.Close();
        }

        public void UpdateById(long id, string name, string image, string description, long adminId)
        {
            connection.Open();

            SqlCommand updateCommand = new SqlCommand(
                "UPDATE Groups SET Name = @Name, AdminId = @AdminId, Description=@Description, Image=@Image WHERE Id = @Id",
                connection
            );

            updateCommand.Parameters.AddWithValue("@Id", id);
            updateCommand.Parameters.AddWithValue("@Name", name);
            updateCommand.Parameters.AddWithValue("@AdminId", adminId);
            updateCommand.Parameters.AddWithValue("@Description", description);
            updateCommand.Parameters.AddWithValue("@Image", image);
            updateCommand.ExecuteNonQuery();

            connection.Close();
        }
    }
}
