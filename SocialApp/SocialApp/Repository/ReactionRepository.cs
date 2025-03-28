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
    class ReactionRepository
    {
        private string loginString = "Data Source=ATHOS;" +
            "Initial Catalog=ISSDB;" +
            "Integrated Security=True;" +
            "TrustServerCertificate=True";
        private SqlConnection connection;

        public ReactionRepository()
        {
            this.connection = new SqlConnection(loginString);
        }

        public List<Reaction> GetAll()
        {
            connection.Open();
            List<Reaction> reactions = new List<Reaction>();

            SqlCommand selectCommand = new SqlCommand("SELECT * FROM Reactions", connection);
            SqlDataReader reader = selectCommand.ExecuteReader();
            while (reader.Read())
            {
                Reaction reaction = new Reaction
                {
                    UserId = reader.GetInt64(reader.GetOrdinal("UserId")),
                    PostId = reader.GetInt64(reader.GetOrdinal("PostId")),
                    Type = (ReactionType)reader.GetInt32(reader.GetOrdinal("Type"))
                };
                reactions.Add(reaction);
            }

            reader.Close();
            connection.Close();
            return reactions;
        }

        public List<Reaction> GetByPost(long postId)
        {
            connection.Open();

            List<Reaction> reactions = new List<Reaction>();
            SqlCommand selectCommand = new SqlCommand(
                "SELECT * FROM Reactions WHERE PostId = @PostId",
                connection
            );
            selectCommand.Parameters.AddWithValue("@PostId", postId);
            SqlDataReader reader = selectCommand.ExecuteReader();
            while (reader.Read())
            {
                Reaction reaction = new Reaction
                {
                    UserId = reader.IsDBNull(reader.GetOrdinal("UserId")) ? 0 : reader.GetInt64(reader.GetOrdinal("UserId")),
                    PostId = reader.IsDBNull(reader.GetOrdinal("PostId")) ? 0 : reader.GetInt64(reader.GetOrdinal("PostId")),
                    Type = reader.IsDBNull(reader.GetOrdinal("ReactionType")) ? ReactionType.Like : (ReactionType)reader.GetInt32(reader.GetOrdinal("ReactionType"))
                };
                reactions.Add(reaction);
            }
            reader.Close();
            connection.Close();
            return reactions;
        }

        public Reaction GetByUserAndPost(long userId, long postId)
        {
            connection.Open();
            Reaction reaction = null;

            SqlCommand selectCommand = new SqlCommand("SELECT * FROM Reactions WHERE UserId = @UserId AND PostId = @PostId", connection);
            selectCommand.Parameters.AddWithValue("@UserId", userId);
            selectCommand.Parameters.AddWithValue("@PostId", postId);

            SqlDataReader reader = selectCommand.ExecuteReader();
            if (reader.Read())
            {
                reaction = new Reaction
                {
                    UserId = reader.GetInt64(reader.GetOrdinal("UserId")),
                    PostId = reader.GetInt64(reader.GetOrdinal("PostId")),
                    Type = (ReactionType)reader.GetInt32(reader.GetOrdinal("ReactionType"))
                };
            }

            reader.Close();
            connection.Close();
            return reaction;
        }

        public void Save(Reaction entity)
        {
            connection.Open();

            SqlCommand insertCommand = new SqlCommand(
                "INSERT INTO Reactions (UserId, PostId, ReactionType) VALUES (@UserId, @PostId, @Type)",
                connection
            );
            insertCommand.Parameters.AddWithValue("@UserId", entity.UserId);
            insertCommand.Parameters.AddWithValue("@PostId", entity.PostId);
            insertCommand.Parameters.AddWithValue("@Type", (int)entity.Type);

            insertCommand.ExecuteNonQuery();

            connection.Close();
        }

        public void UpdateByUserAndPost(long userId, long postId, ReactionType type)
        {
            connection.Open();

            SqlCommand updateCommand = new SqlCommand(
                "UPDATE Reactions SET ReactionType = @Type WHERE UserId = @UserId AND PostId = @PostId",
                connection
            );

            updateCommand.Parameters.AddWithValue("@UserId", userId);
            updateCommand.Parameters.AddWithValue("@PostId", postId);
            updateCommand.Parameters.AddWithValue("@Type", (int)type);
            updateCommand.ExecuteNonQuery();

            connection.Close();
        }

        public void DeleteByUserAndPost(long userId, long postId)
        {
            connection.Open();

            SqlCommand deleteCommand = new SqlCommand("DELETE FROM Reactions WHERE UserId = @UserId AND PostId = @PostId", connection);
            deleteCommand.Parameters.AddWithValue("@UserId", userId);
            deleteCommand.Parameters.AddWithValue("@PostId", postId);
            deleteCommand.ExecuteNonQuery();

            connection.Close();
        }
    }
}
