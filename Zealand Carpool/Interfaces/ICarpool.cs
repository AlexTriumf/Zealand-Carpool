﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Interfaces
{
    /// <summary>
    /// Written by Andreas
    /// </summary>
    public interface ICarpool
    {
        Task<bool> AddCarpool(Carpool carpool);
        Task<List<Branch>> GetBranches();

        Task<Carpool> GetCarpool(int idCarpool);
        Task<Dictionary<int,Carpool>> GetAllCarpools(DateTime date,string search);
        Task<Dictionary<int, Carpool>> GetAllCarpools(Guid userId);
        Task<Dictionary<int,Carpool>> GetAllCarpools(DateTime date);
        Task<Dictionary<int, Carpool>> GetAllCarpoolsAdmin(DateTime date, string search);
        Task<bool> DeleteCarpool(int id);
        Task<Dictionary<Guid,Passenger>> GetPassengers(Carpool carpool);
        Task<List<Passenger>> GetPassengersAdmin(string search);
        Task<bool> AddPassenger(User user, Carpool carpool);
        Task<bool> DeletePassenger(User user, Carpool carpool);
        Task<bool> UpdatePassenger(Guid userId, int carpoolId);
        Carpool MakeCarpool(SqlDataReader sqlReader);
    }
}
