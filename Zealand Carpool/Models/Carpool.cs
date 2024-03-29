﻿using System;
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
        public Branch Branch { get; set; }
        [Required]
        public int PassengerSeats { get; set; }
        public bool HomeAdress { get; set; }
        public User Driver { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<Guid,Passenger> Passengerlist {get;set;}
        public string Details { get; set; }

        public Carpool() { }

        public Carpool(int carpoolId, Branch branch, int passengerSeats,
            User driver, DateTime date, string details, bool homeA)
        {
            CarpoolId = carpoolId;
            Branch = branch;
            PassengerSeats = passengerSeats;
            Driver = driver;
            Date = date;
            Details = details;
            HomeAdress = homeA;
        }

        public override string ToString()
        {
            return
                $"Carpool: {CarpoolId}, Branch: {Branch.BranchName}, " +
                $"PassengerSeats: {PassengerSeats}, DriverId: {Driver.Id.ToString()}, Date: {Date}";
        }
    }
}
