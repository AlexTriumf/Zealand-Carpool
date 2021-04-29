using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Services
{
    public class ChatDabase 
    {

        string _connString = "Data Source=andreas-zealand-server-dk.database.windows.net;Initial Catalog=Andreas-database;User ID=Andreas;Password=SecretPassword!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        string _createChat = "insert into Chat (UserOneId,UserTwoId) Values (@userone, @usertwo)";

        string _getChat = "";


        public Task<bool> AddChat(User userOne, User userTwo)
        {
            Task task = Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(_createChat, conn))
                    {
                        cmd.Parameters.AddWithValue("@userone", userOne.Id);
                        cmd.Parameters.AddWithValue("@usertwo", userTwo.Id);
                        
                        cmd.ExecuteNonQuery();
                    }
                }
            });
            return Task.FromResult(task.IsCompletedSuccessfully);
        }

        public Task<bool> HasAChat(User userOne, User userTwo)
        {
            Task<bool> task = Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(_getChat, conn))
                    {
                        cmd.Parameters.AddWithValue("@userone", userOne.Id);
                        cmd.Parameters.AddWithValue("@usertwo", userTwo.Id);
                        SqlDataReader reader = cmd.ExecuteReader();
                        Chat chat = MakeChat(reader);

                        cmd.ExecuteNonQuery();
                        if (chat.ChatId is -1) return false;
                    }
                }
                return false;
            });
            return task;
        }

        public List<ChatText> GetChat(User userOne, User userTwo)
        {
            throw new NotImplementedException();
        }

        public Chat MakeChat(SqlDataReader reader)
        {
            Chat chat = new Chat();
            chat.ChatId = reader.GetInt32(0);
            chat.UserOne.Id = reader.GetGuid(1);
            chat.UserTwo.Id = reader.GetGuid(2);
            return chat;
        }
    }
}
