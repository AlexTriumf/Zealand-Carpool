using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Zealand_Carpool.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public User LinkedUser { get; set; }
        [Required, MaxLength(500, ErrorMessage = "Beskeden er tom eller også overstiger du 500 tegn "), MinLength(1)]
        public string Text { get; set; }

        public Comment () { }

        public Comment(int newId, User newLinkedUser, string newText)
        {
            Id = newId;
            LinkedUser = newLinkedUser;
            Text = newText;
        }

        public override string ToString()
        {
            return $"Id: {Id}, LinkedUser: {LinkedUser}, Text: {Text}";
        }
    }
}
