using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Services
{
    public class ChatDabase : IChat
    {

        string _connString =
            "Data Source=andreas-zealand-server-dk.database.windows.net;Initial Catalog=Andreas-database;User ID=Andreas;Password=SecretPassword!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        string _createChat = "insert into Chat (UserOneId,UserTwoId) Values (@userone, @usertwo)";

        string _sendChatText =
            "insert into ChatText (ChatId,UserId,Content,TimeStamp) Values (@id, @user, @text, @Timestamp)";

        string _getChat = "Select * From Chat Where (UserOneId = @idone and UserTwoId = @idtwo) OR (UserTwoId = @idone and UserOneId = @idtwo)";
        string _getChatText = "Select * From ChatText Where ChatId = @id";
       

        public Task<bool> AddChat(Guid userOne, Guid userTwo)
        {
            Task task = Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(_createChat, conn))
                    {
                        cmd.Parameters.AddWithValue("@userone", userOne);
                        cmd.Parameters.AddWithValue("@usertwo", userTwo);

                        cmd.ExecuteNonQuery();
                    }
                }
            });
            return Task.FromResult(task.IsCompletedSuccessfully);
        }

        public Task<bool> HasAChat(Guid userOne, Guid userTwo)
        {
            Task<bool> task = Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(_getChat, conn))
                    {
                        cmd.Parameters.AddWithValue("@idone", userOne);
                        cmd.Parameters.AddWithValue("@idtwo", userTwo);
                        SqlDataReader reader = cmd.ExecuteReader();
                        reader.Read();

                        if (!reader.HasRows) return false;
                        Chat chat = MakeChat(reader);

                    }
                    return true;
                }

            });
            return task;
        }

        public Task SendChat(ChatText chatText, int chatId)
        {


            Task task = Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(_sendChatText, conn))
                    {

                        cmd.Parameters.AddWithValue("@id", chatId);
                        cmd.Parameters.AddWithValue("@user", chatText.user.Id);
                        cmd.Parameters.AddWithValue("@text", chatText.message);
                        cmd.Parameters.AddWithValue("@Timestamp", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
            });
            return task;
        }

        public Task<List<ChatText>> GetChat(Guid userOne, Guid userTwo)
        {
            Chat chat = new Chat();
            List<ChatText> chatText = new List<ChatText>();
            Task<List<ChatText>> task = Task.Run(async () =>
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(_getChat, conn))
                    {

                        cmd.Parameters.AddWithValue("@idone", userOne);
                        cmd.Parameters.AddWithValue("@idtwo", userTwo);
                        SqlDataReader reader = cmd.ExecuteReader();
                        reader.Read();
                        chat = MakeChat(reader);

                        cmd.Dispose();
                        reader.Close();
                    }



                    using (SqlCommand cmd = new SqlCommand(_getChatText, conn))
                    {

                        cmd.Parameters.AddWithValue("@id", chat.ChatId);

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            User user1 = new User();
                            user1 = await new UserDatabaseAsync().GetUser(reader.GetGuid(1));
                            chatText.Add(new ChatText { user = user1, message = reader.GetString(2), TimeStamp = reader.GetDateTime(3) });
                        }
                    }
                }
                return chatText;
            });

            return task;
        }

        public Chat MakeChat(SqlDataReader reader)
        {
            Chat chat = new Chat();
            chat.UserOne = new User();
            chat.UserTwo = new User();
            chat.ChatId = reader.GetInt32(0);
            chat.UserOne.Id = reader.GetGuid(1);
            return chat;
        }

        public Task<Chat> GetChatId(Guid UserOneId, Guid UserTwoId)
        {
            Task<Chat> task = Task.Run(() =>
            {


                using (SqlCommand cmd = new SqlCommand(_getChat, DatabaseCon.Instance.SqlConnection()))
                {

                    cmd.Parameters.AddWithValue("@idone", UserOneId);
                    cmd.Parameters.AddWithValue("@idtwo", UserTwoId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Chat chat = new Chat();

                    while (reader.Read())
                    {
                        chat.ChatId = reader.GetInt32(0);
                    }

                    return chat;
                }

            });

            return task;
        }
    }
}
