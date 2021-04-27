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
        public string Branch { get; set; }
        public string HomeAddress { get; set; }
        [Required, Range(1,9) ]
        public int PassengerSeats { get; set; }
        public List<User> PassengerList { get; set; }
        public Guid DriverId { get; set; }
        public DateTime Date { get; set; }

        public Carpool() { }

        public Carpool(int carpoolId, string branch, string homeAddress, int passengerSeats,
            List<User> passengerList, Guid driverId, DateTime date)
        {
            CarpoolId = carpoolId;
            Branch = branch;
            HomeAddress = homeAddress;
            PassengerSeats = passengerSeats;
            PassengerList = passengerList;
            DriverId = driverId;
            Date = date;
        }

        public override string ToString()
        {
            return
                $"Carpool: {CarpoolId}, Branch: {Branch}, HomeAddress: {HomeAddress}, " +
                $"PassengerSeats: {PassengerSeats}, Passengers: {PassengerList.Count}, DriverId: {DriverId}, Date: {Date}";
        }
    }
}
