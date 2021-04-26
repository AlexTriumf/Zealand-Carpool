using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;
using Zealand_Carpool.Pages.LoginPage;
using Zealand_Carpool.Services;

namespace Zealand_Carpool.Services
{
    public class CarpoolDatabase : ICarpool
    {
        private string createCarpool = "INSERT INTO Carpool (UserId, Branch, PassengerSeats, HomeAddress, Date) " +
                                       "VALUES (@UserId, @Branch, @PassengerSeats, @HomeAddress, @Date)";

        private string getCarpool = "SELECT * FROM Carpool WHERE CarpoolId = @CarpoolId";

        private string deleteUser = "DELETE FROM Carpool WHERE CarpoolId = @ID";

        public Task<bool> AddCarpool(Carpool carpool)
        {
            SqlConnection databaseCon = new DatabaseCon().GetConnection();
            Task task = new Task(() =>
            {
                using (SqlCommand cmd = new SqlCommand(createCarpool, databaseCon))
                {
                    cmd.Parameters.AddWithValue("@UserId", carpool.DriverId);
                    cmd.Parameters.AddWithValue("@Branch", carpool.Branch);
                    cmd.Parameters.AddWithValue("@PassengerSeats", carpool.PassengerSeats);
                    cmd.Parameters.AddWithValue("@HomeAddress", carpool.HomeAddress);
                    cmd.Parameters.AddWithValue("@Date", carpool.Date);

                    cmd.ExecuteNonQuery();
                }
            });
            databaseCon.Close();
            return Task.FromResult(task.IsCompletedSuccessfully);
        }

        public Task<Carpool> GetCarpool(int IdCarpool)
        {
            SqlConnection databaseCon = new DatabaseCon().GetConnection();
            Task task = new Task(() =>
            {
                using (SqlCommand cmd = new SqlCommand(getCarpool, databaseCon))
                {
                    cmd.Parameters.AddWithValue("@CarpoolId", IdCarpool);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Carpool carpool = MakeCarpool(reader);
                    return carpool;
                }
            });
            databaseCon.Close();
            //Sidder fast her :( 26/04/2021
            return carpool;
        }

        public Task<bool> DeleteCarpool(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddPassenger(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePassenger(User user)
        {
            throw new NotImplementedException();
        }

        public Carpool MakeCarpool(SqlDataReader sqlReader)
        {
            Carpool carpool = new Carpool();
            carpool.CarpoolId = sqlReader.GetInt32(0);
            carpool.DriverId = sqlReader.GetGuid(1);
            carpool.Branch = sqlReader.GetString(2);
            carpool.PassengerSeats = sqlReader.GetInt32(3);
            carpool.HomeAddress = sqlReader.GetString(4);
            carpool.Date = sqlReader.GetDateTime(5);
            return carpool;
        }
    }
}
