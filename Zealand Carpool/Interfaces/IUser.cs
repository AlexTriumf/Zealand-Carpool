using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Interfaces
{
    interface IUser
    {
        bool AddUser(User user);
        User GetUser(Guid id);
        User GetUser(string email, string password);
        bool DeleteUser(Guid id);
        bool UpdateUser(Guid id, User user);
    }
}
