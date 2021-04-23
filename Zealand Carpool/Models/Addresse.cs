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
    public class Addresse
    {
        [Required, StringLength(255), MinLength(1)]
        public string StreetName { get; set; }
        [Required, MinLength(1),MaxLength(255)]
        public int StreetNumber { get; set; }
        [Required, MinLength(3),MaxLength(4)]
        public int Postalcode { get; set; }
        [Required, StringLength(30)]
        public string CityName { get; set; }

        public Addresse() { }
        public Addresse(string newStreetName, int newStreetNumber, int newPostalcode, string newCityName)
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
