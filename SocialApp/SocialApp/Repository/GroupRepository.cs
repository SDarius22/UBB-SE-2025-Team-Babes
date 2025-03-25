﻿using Microsoft.Data.SqlClient;
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
        private string loginString = "Data Source=DESKTOP-CL1KD74\\SQLEXPRESS01;Initial Catalog=SocialApp;Integrated Security=True;TrustServerCertificate=True";
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
                    AdminId = reader.GetInt64(reader.GetOrdinal("AdminId"))
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
                    AdminId = reader.GetInt64(reader.GetOrdinal("AdminId"))
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
                "INSERT INTO Groups (Name, AdminId) VALUES (@Name, @AdminId)",
                connection
            );
            insertCommand.Parameters.AddWithValue("@Name", entity.Name);
            insertCommand.Parameters.AddWithValue("@AdminId", entity.AdminId);
            insertCommand.ExecuteNonQuery();

            connection.Close();
        }

        public void UpdateById(long id, string name, long adminId)
        {
            connection.Open();

            SqlCommand updateCommand = new SqlCommand(
                "UPDATE Groups SET Name = @Name, AdminId = @AdminId WHERE Id = @Id",
                connection
            );

            updateCommand.Parameters.AddWithValue("@Id", id);
            updateCommand.Parameters.AddWithValue("@Name", name);
            updateCommand.Parameters.AddWithValue("@AdminId", adminId);
            updateCommand.ExecuteNonQuery();

            connection.Close();
        }
    }
}
