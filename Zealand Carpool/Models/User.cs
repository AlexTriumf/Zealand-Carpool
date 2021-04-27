using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace Zealand_Carpool.Models
{
    /// <summary>
    /// A model class of a User
    /// Made by Andreas
    /// </summary>
    public class User
    {
        public Guid Id { get; set; }
        [Required, StringLength(100,ErrorMessage ="For mange tegn"), MinLength(1, ErrorMessage = "For lidt tegn")]
        public string Name { get; set; }
        [Required, StringLength(100, ErrorMessage = "For mange tegn"), MinLength(1, ErrorMessage = "For lidt tegn")]
        public string Surname { get; set; }
       
        public List<Address> AddressList { get; set; }
       [Required, StringLength(255, ErrorMessage = "For mange tegn"), MinLength(1, ErrorMessage = "For lidt tegn")]
        public string Email { get; set; }
        
        public UserType UserType { get; set; }
        [Required, StringLength(20, ErrorMessage = "For mange tegn"), MinLength(6, ErrorMessage = "For lidt tegn")]
        public string Phonenumber { get; set; }
        [Required, StringLength(255, ErrorMessage = "For mange tegn"), MinLength(8, ErrorMessage = "For lidt tegn")]
        public string Password { get; set; }

        public User() { }
        public User(Guid newId, string newName, string newSurname, List<Address> newAddresses, string newEmail,
                UserType newUserType, string newPhonenumber, string newPassword)
        {
                Id = newId;
                Name = newName;
                Surname = newSurname;
                AddressList = newAddresses;
                Email = newEmail;
                UserType = newUserType;
                Phonenumber = newPhonenumber;
                Password = newPassword;
        }
        public override string ToString()
        {
            System.Text.StringBuilder adressesString = new System.Text.StringBuilder();
            foreach (Address address in AddressList)
            {
                adressesString.Append(address);
            }
            return $"User id: {Id}, name: {Name}, surname: {Surname}, addresses: {adressesString.ToString()}, email: {Email}" +
                   $"user Type: {UserType}, phone number: {Phonenumber}";
        }
    }
}