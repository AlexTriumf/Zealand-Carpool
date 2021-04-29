using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zealand_Carpool.Models
{
    /// <summary>
    /// A Model class of Addresses
    /// With a ToString method that returns it all
    /// made by Andreas
    /// </summary>
    public class Address
    {
        [Required, StringLength(255, ErrorMessage = "For mange tegn"), MinLength(1, ErrorMessage = "For lidt tegn")]
        public string StreetName { get; set; }
        [Required, MinLength(1, ErrorMessage = "For lidt tegn"),StringLength(255, ErrorMessage = "For mange tegn")]
        public string StreetNumber { get; set; }
        [Required]
        public int Postalcode { get; set; }
        [Required, StringLength(30, ErrorMessage = "For mange tegn")]
        public string CityName { get; set; }

        public double Latitude { get; set; }
        public double Longtitude { get; set; }

        public Address() { }
        public Address(string newStreetName, string newStreetNumber, int newPostalcode, string newCityName)
        {
            StreetName = newStreetName;
            StreetNumber = newStreetNumber;
            Postalcode = newPostalcode;
            CityName = newCityName;
        }


        public override string ToString()
        {
            return $"Postal code: {Postalcode}, City name: {CityName}, Street name: {StreetName}, Street Number: {StreetNumber}";
        }

        
    }
}

