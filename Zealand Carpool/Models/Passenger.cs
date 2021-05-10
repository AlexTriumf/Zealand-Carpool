using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zealand_Carpool.Models
{
    /// <summary>
    /// A passenger class
    /// made by Andreas
    /// </summary>
    public class Passenger
    {
        public Carpool Carpool { get; set; }
        public User User { get; set; }
        public bool IsAccepted { get; set; }

        public Passenger() { User = new User(); Carpool = new Carpool(); }

        public Passenger(Carpool newCarpool, User newUser, bool newIsAccepted)
        {
            Carpool = newCarpool;
            User = newUser;
            IsAccepted = newIsAccepted;
        }
    }
}
