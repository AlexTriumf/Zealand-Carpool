using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zealand_Carpool.Models
{
    // Made by Andreas
    public class Statistic
    {
        public Carpool Carpool { get; set; }
        public Passenger Passenger {get;set;}
        public int NumberOfTimes { get; set; }

        public Statistic() { }

        public Statistic(Carpool newCarpool,Passenger newPassenger, int newNumber)
        {
            Carpool = newCarpool;
            Passenger = newPassenger;
            NumberOfTimes = newNumber;
        }
    }
}
