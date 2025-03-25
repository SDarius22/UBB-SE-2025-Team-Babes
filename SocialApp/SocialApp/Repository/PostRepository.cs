using Microsoft.Data.SqlClient;
using SocialApp.Entities;
using SocialApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApp.Repository
{
    public class PostRepository
    {
        private string loginString = "Data Source=DESKTOP-CL1KD74\\SQLEXPRESS01;Initial Catalog=SocialApp;Integrated Security=True;TrustServerCertificate=True";
        private SqlConnection connection;

        public PostRepository(string loginString)
        {
            this.loginString = loginString;
            this.connection = new SqlConnection(loginString);
        }

        public List<Post> GetAll()
        {
            connection.Open();
            List<Post> posts = new List<Post>();

            SqlCommand selectCommand = new SqlCommand("SELECT * FROM Posts", connection);
            SqlDataReader reader = selectCommand.ExecuteReader();
            while (reader.Read())
            {
                Post post = new Post
                {
                    Id = reader.GetInt64(reader.GetOrdinal("Id")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    UserId = reader.GetInt64(reader.GetOrdinal("UserId")),
                    GroupId = reader.GetInt64(reader.GetOrdinal("GroupId")),
                    Visibility = (PostVisibility)reader.GetInt32(reader.GetOrdinal("PostVisibility")),
                    Tag = (PostTag)reader.GetInt32(reader.GetOrdinal("PostTag"))
                };
                posts.Add(post);
            }

            reader.Close();
            connection.Close();
            return posts;
        }

        public Post GetById(long id)
        {
            connection.Open();
            Post post = null;

            SqlCommand selectCommand = new SqlCommand("SELECT * FROM Posts WHERE Id = @Id", connection);
            selectCommand.Parameters.AddWithValue("@Id", id);

            SqlDataReader reader = selectCommand.ExecuteReader();
            if (reader.Read())
            {
                post = new Post
                {
                    Id = reader.GetInt64(reader.GetOrdinal("Id")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    UserId = reader.GetInt64(reader.GetOrdinal("UserId")),
                    GroupId = reader.GetInt64(reader.GetOrdinal("GroupId")),
                    Visibility = (PostVisibility)reader.GetInt32(reader.GetOrdinal("PostVisibility")),
                    Tag = (PostTag)reader.GetInt32(reader.GetOrdinal("PostTag"))
                };
            }

            reader.Close();
            connection.Close();
            return post;
        }

        public void Save(Post entity)
        {
            connection.Open();

            SqlCommand insertCommand = new SqlCommand(
                "INSERT INTO Posts (Title, Description, CreatedDate, UserId, PostVisibility, GroupId, PostTag) VALUES (@Title, @Description, @CreatedDate, @UserId, @PostVisibility, @GroupId, @PostTag)",
                connection
            );
            insertCommand.Parameters.AddWithValue("@Title", entity.Title);
            insertCommand.Parameters.AddWithValue("@Description", entity.Description ?? (object)DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedDate", entity.CreatedDate);
            insertCommand.Parameters.AddWithValue("@UserId", entity.UserId);
            insertCommand.Parameters.AddWithValue("@PostVisibility", (int)entity.Visibility);
            insertCommand.Parameters.AddWithValue("@GroupId", entity.GroupId);
            insertCommand.Parameters.AddWithValue("@PostTag", (int)entity.Tag);

            insertCommand.ExecuteNonQuery();

            connection.Close();
        }

        public void UpdateById(long id, string title, string description, DateTime createdDate, long userId, PostVisibility visibility, long groupId, PostTag tag)
        {
            connection.Open();

            SqlCommand updateCommand = new SqlCommand(
                "UPDATE Posts SET Title = @Title, Description = @Description, CreatedDate = @CreatedDate, UserId = @UserId, PostVisibility = @PostVisibility, GroupId = @GroupId, PostTag = @PostTag WHERE Id = @Id",
                connection
            );

            updateCommand.Parameters.AddWithValue("@Id", id);
            updateCommand.Parameters.AddWithValue("@Title", title);
            updateCommand.Parameters.AddWithValue("@Description", description ?? (object)DBNull.Value);
            updateCommand.Parameters.AddWithValue("@CreatedDate", createdDate);
            updateCommand.Parameters.AddWithValue("@UserId", userId);
            updateCommand.Parameters.AddWithValue("@PostVisibility", (int)visibility);
            updateCommand.Parameters.AddWithValue("@GroupId", groupId);
            updateCommand.Parameters.AddWithValue("@PostTag", (int)tag);

            updateCommand.ExecuteNonQuery();

            connection.Close();
        }

        public void DeleteById(long id)
        {
            connection.Open();

            SqlCommand deleteCommand = new SqlCommand("DELETE FROM Posts WHERE Id = @Id", connection);
            deleteCommand.Parameters.AddWithValue("@Id", id);
            deleteCommand.ExecuteNonQuery();

            connection.Close();
        }
    }
}
