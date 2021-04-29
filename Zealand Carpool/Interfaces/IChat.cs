using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Interfaces
{
    /// <summary>
    /// An Interface dedicated to Chat
    /// made by Andreas
    /// </summary>
    public interface IChat
    {
        
        public Task<bool> AddChat(Guid userOne, Guid userTwo);
        public Task<List<ChatText>> GetChat(Guid userOne, Guid userTwo);
        public Task<bool> HasAChat(Guid userone, Guid userTwo);
        public Task SendChat(ChatText chatText, int chatId);
        public Chat MakeChat(System.Data.SqlClient.SqlDataReader reader);
    }
}
