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
        [Required, MaxLength(500, ErrorMessage = "Beskeden er tom eller også overstiger du 500 tegn ")]
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
            return $"{nameof(Id)}: {Id}, {nameof(LinkedUser)}: {LinkedUser}, {nameof(Text)}: {Text}";
        }
    }
}
