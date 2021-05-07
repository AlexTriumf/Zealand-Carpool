using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Interfaces
{
    public interface IUser
    {
        Task<bool> AddUser(User user);
        Task<User> GetUser(Guid id);
        Task<User> GetUser(string email, string password);
        Task<User> GetUserID(string email, string password);
        Task<Dictionary<Guid, User>> GetAllUsers();
        Task<bool> DeleteUser(Guid id);
        Task<bool> UpdateUser(Guid id, User user);
        User MakeUser(SqlDataReader sqlreader);
        //Written by Malte
        public List<User> SearchUsers(string name);
    }
}
