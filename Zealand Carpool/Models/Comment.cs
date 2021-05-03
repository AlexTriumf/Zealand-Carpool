using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Zealand_Carpool.Models
{
    public class Comment
    {
        //Lavet af Aleksandar
        public int Id { get; set; }
        public User UserPostID { get; set; }
        public User UserID { get; set; }
        [Required, MaxLength(500, ErrorMessage = "Beskeden er tom eller også overstiger du 500 tegn "), MinLength(1)]
        public string Text { get; set; }

        public Comment () { }

        public Comment(User newUserPostID, User newUserID, string newText)
        {
            UserPostID = newUserPostID;
            UserID = newUserID;
            Text = newText;
        }

        public override string ToString()
        {
            return $"Id: {Id}, UserPostID: {UserPostID}, UserID: {UserID}, Text: {Text}";
        }
    }
}
