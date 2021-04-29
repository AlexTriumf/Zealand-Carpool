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
        Task<bool> AddCarpool(Carpool carpool);
        Task<Carpool> GetCarpool(int idCarpool);
        Task<bool> DeleteCarpool(int id);
        Task<bool> AddPassenger(User user);
        Task<bool> DeletePassenger(User user);
        Carpool MakeCarpool(SqlDataReader sqlReader);
    }
}
