using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Zealand_Carpool.Models
{
    public class ChatText
    {
        [Required, MaxLength(500, ErrorMessage = "Beskeden er tom eller også overstiger du 500 tegn "), MinLength(1)]
        public string message { get; set; }
        public User user { get; set; }
    }
}
