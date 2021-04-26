using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Services
{
    public class UserDatabase : IUser
    {
        public bool AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public User GetUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string email, string password)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUser(Guid id, User user)
        {
            throw new NotImplementedException();
        }
    }
}
