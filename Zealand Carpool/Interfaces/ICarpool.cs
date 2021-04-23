using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Interfaces
{
    /// <summary>
    /// Written by Malte
    /// </summary>
    public interface ICarpool
    {
        public void AddDrive(Carpool carpool);
        public Carpool GetCarpool(int id);
        public bool DeleteCarpool(int id);
        public void AddPassenger(User user);
        public bool DeletePassenger(User user);


    }
}
