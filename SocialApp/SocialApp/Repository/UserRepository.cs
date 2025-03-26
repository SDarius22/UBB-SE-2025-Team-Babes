using Microsoft.Data.SqlClient;
using SocialApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApp.Repository
{
    public class UserRepository
    {
        private string loginString = "Data Source=DESKTOP-CL1KD74\\SQLEXPRESS01;Initial Catalog=SocialApp;Integrated Security=True;TrustServerCertificate=True";
        private SqlConnection connection;

        public UserRepository()
        {
            this.connection = new SqlConnection(loginString);
        }

        public UserRepository(string loginString)
        {
            this.loginString = loginString;
            this.connection = new SqlConnection(loginString);
        }

        public List<User> GetAll()
        {
            connection.Open();
            List<User> users = new List<User>();

            SqlCommand selectCommand = new SqlCommand("SELECT * FROM Users", connection);
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
                users.Add(user);
            }

            reader.Close();
            connection.Close();
            return users;
        }

        public List<User> GetUserFollowers(long id)
        {
            connection.Open();
            List<User> users = new List<User>();
            SqlCommand selectCommand = new SqlCommand("SELECT * FROM Users WHERE Id IN (SELECT FollowerId FROM UserFollowers WHERE UserId = @Id)", connection);
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
                users.Add(user);
            }
            reader.Close();
            connection.Close();
            return users;
        }

        public List<User> GetUserFollowing(long id)
        {
            connection.Open();
            List<User> users = new List<User>();
            SqlCommand selectCommand = new SqlCommand("SELECT * FROM Users WHERE Id IN (SELECT UserId FROM UserFollowers WHERE FollowerId = @Id)", connection);
            selectCommand.Parameters.AddWithValue("@Id", id);
            SqlDataReader reader = selectCommand.ExecuteReader();
            while (reader.Read()) {
                User user = new User
                {
                    Id = reader.GetInt64(reader.GetOrdinal("Id")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    Image = reader.GetString(reader.GetOrdinal("Image"))
                };
                users.Add(user);
            }
            reader.Close();
            connection.Close();
            return users;
        }

        public void Follow(long userId, long whoToFollowId)
        {
            connection.Open();
            SqlCommand insertCommand = new SqlCommand("INSERT INTO UserFollowers (UserId, FollowerId) VALUES (@UserId, @FollowerId)", connection);
            insertCommand.Parameters.AddWithValue("@UserId", whoToFollowId);
            insertCommand.Parameters.AddWithValue("@FollowerId", userId);
            insertCommand.ExecuteNonQuery();
            connection.Close();
        }

        public void Unfollow(long userId, long whoToUnfollowId)
        {
            connection.Open();
            SqlCommand deleteCommand = new SqlCommand("DELETE FROM UserFollowers WHERE UserId = @UserId AND FollowerId = @FollowerId", connection);
            deleteCommand.Parameters.AddWithValue("@UserId", whoToUnfollowId);
            deleteCommand.Parameters.AddWithValue("@FollowerId", userId);
            deleteCommand.ExecuteNonQuery();
            connection.Close();
        }

        public User GetByEmail(string email)
        {
            connection.Open();
            User user = null;
            SqlCommand selectCommand = new SqlCommand("SELECT * FROM Users WHERE Email = @Email", connection);
            selectCommand.Parameters.AddWithValue("@Email", email);
            SqlDataReader reader = selectCommand.ExecuteReader();
            if (reader.Read())
            {
                user = new User
                {
                    Id = reader.GetInt64(reader.GetOrdinal("Id")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    Image = reader.GetString(reader.GetOrdinal("Image"))
                };
            }
            reader.Close();
            connection.Close();
            return user;
        }

        public User GetById(long id)
        {
            connection.Open();
            User user = null;

            SqlCommand selectCommand = new SqlCommand("SELECT * FROM Users WHERE Id = @Id", connection);
            selectCommand.Parameters.AddWithValue("@Id", id);

            SqlDataReader reader = selectCommand.ExecuteReader();
            if (reader.Read())
            {
                user = new User
                {
                    Id = reader.GetInt64(reader.GetOrdinal("Id")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    Image = reader.GetString(reader.GetOrdinal("Image"))
                };
            }

            reader.Close();
            connection.Close();
            return user;
        }

        public void Save(User entity)
        {
            connection.Open();

            SqlCommand insertCommand = new SqlCommand(
                "INSERT INTO Users (Username, Email, PasswordHash, Image) VALUES (@Username, @Email, @PasswordHash, @Image)",
                connection
            );
            insertCommand.Parameters.AddWithValue("@Username", entity.Username);
            insertCommand.Parameters.AddWithValue("@Email", entity.Email);
            insertCommand.Parameters.AddWithValue("@PasswordHash", entity.PasswordHash);
            insertCommand.Parameters.AddWithValue("@Image", entity.Image);
            insertCommand.ExecuteNonQuery();

            connection.Close();
        }

        public void UpdateById(long id, string username, string email, string passwordHash, string? image)
        {
            connection.Open();

            SqlCommand updateCommand = new SqlCommand(
                "UPDATE Users SET Username = @Username, Email = @Email, PasswordHash = @PasswordHash, Image=@Image WHERE Id = @Id",
                connection
            );

            updateCommand.Parameters.AddWithValue("@Id", id);
            updateCommand.Parameters.AddWithValue("@Username", username);
            updateCommand.Parameters.AddWithValue("@Email", email);
            updateCommand.Parameters.AddWithValue("@PasswordHash", passwordHash);
            updateCommand.Parameters.AddWithValue("@Image", image);
            updateCommand.ExecuteNonQuery();

            connection.Close();
        }

        public void DeleteById(long id)
        {
            connection.Open();

            SqlCommand deleteCommand = new SqlCommand("DELETE FROM Users WHERE Id = @Id", connection);
            deleteCommand.Parameters.AddWithValue("@Id", id);
            deleteCommand.ExecuteNonQuery();

            connection.Close();
        }
    }
}
