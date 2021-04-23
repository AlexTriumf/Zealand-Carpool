using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Zealand_Carpool.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public User UserOne { get; set; }
        public User UserTwo { get; set; }
        public List<ChatText> ChatText { get; set; }


    }
}
