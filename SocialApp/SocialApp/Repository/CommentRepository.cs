using SocialApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SocialApp.Repository
{
    public class CommentRepository
    {
        private string loginString = "Data Source=DESKTOP-CL1KD74\\SQLEXPRESS01;Initial Catalog=SocialApp;Integrated Security=True;TrustServerCertificate=True";
        private SqlConnection connection;

        public CommentRepository(string loginString)
        {
            this.loginString = loginString;
            this.connection = new SqlConnection(loginString);
        }

        public List<Comment> GetAll()
        {
            connection.Open();
            List<Comment> ans = new List<Comment>();

            SqlCommand selectCommand = new SqlCommand("SELECT * FROM Comments", connection);
            SqlDataReader reader = selectCommand.ExecuteReader();
            while (reader.Read())
            {
                Comment comment = new Comment
                {
                    Id = reader.GetInt64(reader.GetOrdinal("Id")),
                    UserId = reader.GetInt64(reader.GetOrdinal("UserId")),
                    PostId = reader.GetInt64(reader.GetOrdinal("PostId")),
                    Content = reader.GetString(reader.GetOrdinal("Content")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                };
                ans.Add(comment);
            }

            reader.Close();
            connection.Close();
            return ans;
        }

        public void DeleteById(long id)
        {
            connection.Open();

            SqlCommand deleteCommand = new SqlCommand("DELETE FROM Comments WHERE Id = @Id", connection);
            string queryParam = id.ToString();
            deleteCommand.Parameters.AddWithValue("@Id", queryParam);
            deleteCommand.ExecuteNonQuery();

            connection.Close();
        }

        public Comment GetById(long id)
        {
            connection.Open();
            Comment comment = null;

            SqlCommand selectCommand = new SqlCommand("SELECT * FROM Comments WHERE Id = @Id", connection);
            string queryParam = id.ToString();
            selectCommand.Parameters.AddWithValue("@Id", queryParam);

            SqlDataReader reader = selectCommand.ExecuteReader();
            if (reader.Read())
            {
                comment = new Comment
                {
                    Id = reader.GetInt64(reader.GetOrdinal("Id")),
                    UserId = reader.GetInt64(reader.GetOrdinal("UserId")),
                    PostId = reader.GetInt64(reader.GetOrdinal("PostId")),
                    Content = reader.GetString(reader.GetOrdinal("Content")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                };
            }

            reader.Close();
            connection.Close();
            return comment; 
        }

        public void Save(Comment entity)
        {
            connection.Open();

            SqlCommand insertCommand = new SqlCommand(
                "INSERT INTO Comments (UserId, PostId, Content, CreatedDate) VALUES (@UserId, @PostId, @Content, @CreatedDate)",
                connection
            );
            insertCommand.Parameters.AddWithValue("@UserId", entity.UserId);
            insertCommand.Parameters.AddWithValue("@PostId", entity.PostId);
            insertCommand.Parameters.AddWithValue("@Content", entity.Content);
            insertCommand.Parameters.AddWithValue("@CreatedDate", entity.CreatedDate);
            insertCommand.ExecuteNonQuery();

            connection.Close();
        }

        public void UpdateById(long id, string content)
        {
            connection.Open();

            SqlCommand updateCommand = new SqlCommand(
                "UPDATE Comments SET Content = @Content WHERE Id = @Id",
                connection
            );

            updateCommand.Parameters.AddWithValue("@Id", id);
            updateCommand.Parameters.AddWithValue("@Content", content);
            updateCommand.ExecuteNonQuery();

            connection.Close();
        }
    }
}
