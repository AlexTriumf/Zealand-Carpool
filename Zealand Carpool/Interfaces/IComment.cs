using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Interfaces
{
    public interface IComment
    {
        public void AddComment(Comment comment);
        public Comment GetComment(int id);

        public void DeleteComment(int id);
    }
}
