using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        bool AddCarpool(Carpool carpool);
        Carpool GetCarpool(int idCarpool);
        //TODO List<Carpool> GetAllCarpoolsByPostalCode(int postalCode);
        bool DeleteCarpool(int id);
        bool UpdateCarpool(Carpool carpool);
        bool AddPassenger(User user, Carpool carpool);
        bool DeletePassenger(User user, Carpool carpool);
        
        Carpool MakeCarpool(SqlDataReader sqlReader);
    }
}
