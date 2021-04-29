using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Interfaces
{
    public interface IComment
    {
        //Lavet af Aleksandar
        public void AddComment(Comment comment);
        public Comment GetComment(int id);
        public List<Comment> getComments(Guid UserId);

        public void DeleteComment(int id);
    }
}
