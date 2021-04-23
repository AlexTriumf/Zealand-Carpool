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
        [Required, StringLength(100), MinLength(1)]
        public string Name { get; set; }
        [Required, StringLength(100), MinLength(1)]
        public string Surname { get; set; }
        public List<Address> AddressList { get; set; }
        [Required, StringLength(255), MinLength(1)]
        public string Email { get; set; }
        [Required]
        public UserType UserType { get; set; }
        [Required, StringLength(20), MinLength(6)]
        public string Phonenumber { get; set; }
        public User() { }
        public User(Guid newId, string newName, string newSurname, List<Address> newAddresses, string newEmail,
                UserType newUserType, string newPhonenumber)
        {
                Id = newId;
                Name = newName;
                Surname = newSurname;
                AddressList = newAddresses;
                Email = newEmail;
                UserType = newUserType;
                Phonenumber = newPhonenumber;
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