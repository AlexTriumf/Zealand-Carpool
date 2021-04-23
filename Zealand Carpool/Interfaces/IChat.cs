using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Interfaces
{
    public interface IChat
    {
        public void AddChat(User userOne, User userTwo);
        public Chat GetChat(User userOne, User userTwo);
       
    }
}
