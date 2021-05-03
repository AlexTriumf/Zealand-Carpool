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
        public int CarpoolId { get; set; }
        [Required, MinLength(1), MaxLength(255)]
        public Branch Branch { get; set; }
        [Required, Range(1, 9)]
        public int PassengerSeats { get; set; }
        public List<User> PassengerList { get; set; }
        public  User Driver { get; set; }
        public DateTime Date { get; set; }

        public Carpool() { }

        public Carpool(int carpoolId, Branch branch, string homeAddress, int passengerSeats,
            List<User> passengerList, User driver, DateTime date)
        {
            CarpoolId = carpoolId;
            Branch = branch;
            PassengerSeats = passengerSeats;
            PassengerList = passengerList;
            Driver = driver;
            Date = date;
        }

        public void AddPassenger(User user)
        {
            PassengerList.Add(user);
        }

        public bool RemovePassenger(User user)
        {
            return PassengerList.Remove(user);
        }

        public override string ToString()
        {
            return
                $"Carpool: {CarpoolId}, Branch: {Branch.BranchName}, " +
                $"PassengerSeats: {PassengerSeats}, Passengers: {PassengerList.Count}, DriverId: {Driver.Id.ToString()}, Date: {Date}";
        }
    }
}
