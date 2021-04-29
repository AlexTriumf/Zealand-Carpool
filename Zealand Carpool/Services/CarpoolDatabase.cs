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
<<<<<<< Updated upstream
    public class CarpoolDatabase 
=======

    /// <summary>
    /// Written by Malte
    /// </summary>
    public class CarpoolDatabase : ICarpool
>>>>>>> Stashed changes
    {
        private string _connString = "Data Source=andreas-zealand-server-dk.database.windows.net;Initial Catalog=Andreas-database;User ID=Andreas;Password=SecretPassword!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private string addCarpool = "INSERT INTO Carpool (UserId, Branch, PassengerSeats, HomeAddress, Date) " +
                                       "VALUES (@UserId, @Branch, @PassengerSeats, @HomeAddress, @Date)";
        public bool AddCarpool(Carpool carpool)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(addCarpool, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", carpool.DriverId);
                    cmd.Parameters.AddWithValue("@Branch", carpool.Branch);
                    cmd.Parameters.AddWithValue("@PassengerSeats", carpool.PassengerSeats);
                    cmd.Parameters.AddWithValue("@HomeAddress", carpool.HomeAddress);
                    cmd.Parameters.AddWithValue("@Date", carpool.Date);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                    if (rowsAffected == 1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

<<<<<<< Updated upstream
        public void GetCarpool(int IdCarpool)
=======
        private string getCarpool = "SELECT * FROM Carpool WHERE CarpoolId = @CarpoolId";
        public Carpool GetCarpool(int IdCarpool)
>>>>>>> Stashed changes
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(getCarpool, conn))
                {
                    cmd.Parameters.AddWithValue("@CarpoolId", IdCarpool);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Carpool carpool = MakeCarpool(reader);
                    conn.Close();
                    if (carpool != null)
                    {
                        return carpool;
                    }
                    throw new KeyNotFoundException("The carpool was not found in the database");
                }
<<<<<<< Updated upstream
            });
            databaseCon.Close();
            //Sidder fast her :( 26/04/2021
            return carpool;*/
            throw new NotImplementedException();

=======
            }
>>>>>>> Stashed changes
        }

        /*private string postalCode = "SELECT * FROM Carpool WHERE "
        public List<Carpool> GetAllCarpoolsByPostalCode(int postalCode)
        {
        }*/

        private string deleteCarpool = "DELETE FROM Carpool WHERE CarpoolId = @CarpoolId";
        public bool DeleteCarpool(int carpoolId)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(deleteCarpool, conn))
                {
                    cmd.Parameters.AddWithValue("@CarpoolId", carpoolId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                    if (rowsAffected == 1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private string updateCarpool = "UPDATE Carpool SET Branch = @Branch, PassengerSeats = @PassengerSeats," +
                                       " HomeAddress = @HomeAddress, Date = @Date WHERE CarpoolId = @CarpoolId";
        public bool UpdateCarpool(Carpool carpool)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(updateCarpool, conn))
                {
                    cmd.Parameters.AddWithValue("@Branch", carpool.Branch);
                    cmd.Parameters.AddWithValue("@PassengerSeats", carpool.PassengerSeats);
                    cmd.Parameters.AddWithValue("@HomeAddress", carpool.HomeAddress);
                    cmd.Parameters.AddWithValue("@Date", carpool.Date);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                    if (rowsAffected == 1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private string addPassenger = "INSERT INTO Passengers(CarpoolId, UserId) VALUES (@CarpoolId, @UserId)";
        public bool AddPassenger(User user, Carpool carpool)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();

                using(SqlCommand cmd = new SqlCommand(addPassenger, conn))
                {
                    cmd.Parameters.AddWithValue("@CarppolId", carpool.CarpoolId);
                    cmd.Parameters.AddWithValue("@UserId", user.Id);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                    if (rowsAffected == 1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private string deletePassenger = "DELETE FROM Passengers WHERE CarpoolId = @CarpoolId, UserId = @UserId"; 
        public bool DeletePassenger(User user, Carpool carpool)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(deletePassenger, conn))
                {
                    cmd.Parameters.AddWithValue("@CarpoolId", carpool.CarpoolId);
                    cmd.Parameters.AddWithValue("@UserId", user.Id);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                    if (rowsAffected == 1)
                    {
                        return true;
                    }
                }
            }

            return false;
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
