using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zealand_Carpool.Models
{
    /// <summary>
    /// Written by Malte
    /// </summary>
    public class Carpool
    {
        public int CarpoolId { get; set;}
        [Required, MinLength(1), MaxLength(255)]
        public string AddressFrom { get; set; }
        public string AddressTo { get; set; }
        [Required, Range(1,9) ]
        public int PassengerSeats { get; set; }
        public List<User> PassengerList { get; set; }
        public User DriverId { get; set; }
        public DateTime Date { get; set; }

        public Carpool() { }

        public Carpool(int carpoolId, string addressFrom, string addressTo, int passengerSeats,
            List<User> passengerList, User driver, DateTime date)
        {
            CarpoolId = carpoolId;
            AddressFrom = addressFrom;
            AddressTo = addressTo;
            PassengerSeats = passengerSeats;
            PassengerList = passengerList;
            DriverId = driver;
            Date = date;
        }

        public override string ToString()
        {
            return
                $"Carpool: {CarpoolId}, AddressFrom: {AddressFrom}, AddressTo: {AddressTo}, " +
                $"PassengerSeats: {PassengerSeats}, Passengers: {PassengerList.Count}, DriverId: {DriverId.Id.ToString()}, Date: {Date}";
        }
    }
}
