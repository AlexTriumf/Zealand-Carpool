using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;
using Zealand_Carpool.Pages.LoginPage;
using Zealand_Carpool.Services;

/// <summary>
/// Written by Andreas
/// </summary>
namespace Zealand_Carpool.Services
{
    public class CarpoolDatabase : ICarpool
    {
        private string _connString = "Data Source=andreas-zealand-server-dk.database.windows.net;Initial Catalog=Andreas-database;User ID=Andreas;Password=SecretPassword!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private string _addCarpool = "INSERT INTO Carpool (UserId, Branch, PassengerSeats, Date, Detail) " +
                                       "VALUES (@UserId, @Branch, @PassengerSeats, @Date, @Detail)";
        private string _getBranches = "SELECT * FROM Branch";

        private string _getCarpool = "SELECT UserTable.UserId,UserTable.Name,UserTable.Surname,UserTable.Email,UserTable.Phonenumber, " +
                        "AddressList.StreetName,AddressList.Streetnr,AddressList.Latitude, " +
                        "AddressList.Longtitude,PostalCode.City,PostalCode.PostalCode, Carpool.CarpoolId, " +
                        "Carpool.Branch,Carpool.PassengerSeats,Carpool.Date,Branch.BranchName,Carpool.Detail FROM UserTable " +
                        "INNER JOIN AddressList ON UserTable.UserId=AddressList.UserId " +
                        "INNER join PostalCode on AddressList.PostalCode= PostalCode.PostalCode " +
                        "Inner join Carpool on UserTable.UserId = Carpool.UserId " +
                        "inner join Branch on Carpool.Branch = Branch.BranchId " +
                        "WHERE Carpool.CarpoolId = @carpoolId";

        private string _getAllCarpools = "SELECT UserTable.UserId, UserTable.Name, UserTable.Surname, UserTable.Email, UserTable.Phonenumber, " +
                        "AddressList.StreetName,AddressList.Streetnr,AddressList.Latitude," +
                        "AddressList.Longtitude,PostalCode.City,PostalCode.PostalCode, Carpool.CarpoolId," +
                        "Carpool.Branch,Carpool.PassengerSeats,Carpool.Date,Branch.BranchName,Carpool.Detail FROM UserTable " +
                        "INNER JOIN AddressList ON UserTable.UserId=AddressList.UserId " +
                        "INNER join PostalCode on AddressList.PostalCode= PostalCode.PostalCode " +
                        "Inner join Carpool on UserTable.UserId = Carpool.UserId " +
                        "inner join Branch on Carpool.Branch = Branch.BranchId " +
                        "WHERE Carpool.Date > @theDate";

        private string _getAllCarpoolsSearch = "SELECT UserTable.UserId, UserTable.Name, UserTable.Surname, UserTable.Email, UserTable.Phonenumber, " +
                        "AddressList.StreetName,AddressList.Streetnr,AddressList.Latitude," +
                        "AddressList.Longtitude,PostalCode.City,PostalCode.PostalCode, Carpool.CarpoolId," +
                        "Carpool.Branch,Carpool.PassengerSeats,Carpool.Date,Branch.BranchName,Carpool.Detail FROM UserTable " +
                        "INNER JOIN AddressList ON UserTable.UserId=AddressList.UserId " +
                        "INNER join PostalCode on AddressList.PostalCode= PostalCode.PostalCode " +
                        "Inner join Carpool on UserTable.UserId = Carpool.UserId " +
                        "inner join Branch on Carpool.Branch = Branch.BranchId " +
                        "WHERE Carpool.Date > @theDate and Branch.BranchName Like @Search OR AddressList.StreetName Like @Search OR PostalCode.City Like @Search";

        private string _getAllUserCarpools = "SELECT UserTable.UserId, UserTable.Name, UserTable.Surname, UserTable.Email, UserTable.Phonenumber, " +
                        "AddressList.StreetName,AddressList.Streetnr,AddressList.Latitude," +
                        "AddressList.Longtitude,PostalCode.City,PostalCode.PostalCode, Carpool.CarpoolId," +
                        "Carpool.Branch,Carpool.PassengerSeats,Carpool.Date,Branch.BranchName,Carpool.Detail FROM UserTable " +
                        "INNER JOIN AddressList ON UserTable.UserId=AddressList.UserId " +
                        "INNER join PostalCode on AddressList.PostalCode= PostalCode.PostalCode " +
                        "Inner join Carpool on UserTable.UserId = Carpool.UserId " +
                        "inner join Branch on Carpool.Branch = Branch.BranchId";

        private string _deletePassenger = "DELETE FROM Passengers WHERE CarpoolId = @CarpoolId and UserId = @UserId";

        private string _deleteCarpool = "DELETE FROM Carpool WHERE CarpoolId = @CarpoolId";
        private string _addPassenger = "INSERT INTO Passengers(CarpoolId, UserId, Request) VALUES (@CarpoolId, @UserId, @request)";
        private string _getPassengers = "SELECT * FROM Passengers WHERE CarpoolId = @carpoolId";
        private string _updatePassenger = "UPDATE Passengers SET Request = @request WHERE CarpoolId = @carpoolId and UserId = @userid";
        private string _getAllPassengers = "SELECT Passengers.CarpoolId, Passengers.UserId, Passengers.Request FROM Passengers INNER JOIN Carpool on Carpool.CarpoolId = Passengers.CarpoolId WHERE Carpool.Date > @theDate";
        private string _getAllUserPassengers = "SELECT Passengers.CarpoolId, Passengers.UserId, Passengers.Request FROM Passengers INNER JOIN Carpool on Carpool.CarpoolId = Passengers.CarpoolId WHERE Passengers.UserId = @user";
        // lav add-get-del passager gør ligesom tweetr med likes
        //lav exeption til user og dette
        //skal hente alle carpools som en user har lavet nu[Done], skal hente alle carpools en user er med i [Done]
        // den der har lavet en carpool accept, delete carpool/passager [Done]
        // userhistory mangler[Done] layout links, Detaljer på carpool.

        public Task<bool> AddCarpool(Carpool carpool)
        {
            Task<bool> task = Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(_addCarpool, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", carpool.Driver.Id);
                        cmd.Parameters.AddWithValue("@Branch", carpool.Branch.BranchId);
                        cmd.Parameters.AddWithValue("@PassengerSeats", carpool.PassengerSeats);
                        cmd.Parameters.AddWithValue("@Date", carpool.Date);
                        if (carpool.Details is null)
                        {
                            carpool.Details = "";
                        }
                        cmd.Parameters.AddWithValue("@Detail", carpool.Details);
                        
                        int rowsAffected = cmd.ExecuteNonQuery();
                        conn.Close();
                        if (rowsAffected == 1)
                        {
                            return true;
                        }
                        return false;
                        throw new InvalidOperationException("Der blev ikke tilføjet carpool, brugerID: " + carpool.Driver.Id);
                    }
                }
            
            });
            return task;
        }

        public Task<List<Branch>> GetBranches()
        {
            Task<List<Branch>> task = Task.Run(() => { 
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();
                List<Branch> branches = new List<Branch>();

                using (SqlCommand cmd = new SqlCommand(_getBranches, conn))
                {

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                       
                        Branch branch = new Branch();
                        branch.BranchId = reader.GetInt32(0);
                        branch.BranchPostalCode = reader.GetInt32(1);
                        branch.BranchName = reader.GetString(2);
                        branches.Add(branch);
                    }
                        return branches;
                }
            }}); return task;
        }


        
        public Task<Carpool> GetCarpool(int IdCarpool)
        {
            Task<Carpool> task = Task.Run(() =>
            {
                Carpool carpool = new Carpool();
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(_getCarpool, conn))
                    {
                        cmd.Parameters.AddWithValue("@carpoolId", IdCarpool);
                        SqlDataReader reader = cmd.ExecuteReader();
                        reader.Read();
                        
                        carpool = MakeCarpool(reader);
                        cmd.Dispose();
                        reader.Close();
                    }

                    using (SqlCommand cmd = new SqlCommand(_getPassengers, conn))
                    {
                        cmd.Parameters.AddWithValue("@carpoolId", carpool.CarpoolId);
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Passenger passenger = new Passenger();
                            passenger.Carpool = new Carpool();
                            passenger.Carpool.CarpoolId = reader.GetInt32(0);
                            passenger.User = new User();
                            passenger.User.Id = reader.GetGuid(1);
                            passenger.IsAccepted = reader.GetBoolean(2);
                            carpool.Passengerlist.Add(passenger.User.Id,passenger);
                        }
                    }
                } return carpool;
            }); return task;
        }


        

        public Task<bool> DeleteCarpool(int carpoolId)
        {
            Task<bool> task = Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(_deleteCarpool, conn))
                    {
                        cmd.Parameters.AddWithValue("@CarpoolId", carpoolId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        conn.Close();
                        if (rowsAffected == 1)
                        {
                            return true;
                        } else
                        {
                        return false;
                        throw new InvalidOperationException("Der blev ikke slettet carpool id: " + carpoolId );
                        }
                    }
                }
            });

            return task;
        }

        

        public Task<bool> AddPassenger(User user, Carpool carpool)
        {
            Task<bool> task = Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(_addPassenger, conn))
                    {
                        cmd.Parameters.AddWithValue("@CarpoolId", carpool.CarpoolId);
                        cmd.Parameters.AddWithValue("@UserId", user.Id);
                        cmd.Parameters.AddWithValue("@request", 0);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        
                        if (rowsAffected == 1)
                        {
                            return true;
                        }
                        return false;
                        throw new InvalidOperationException("Der blev ikke tilføjet passager id: " + user.Id);
                    }
                }
            });

            return task;
        }

        public Task<bool> DeletePassenger(User user, Carpool carpool)
        {
            Task<bool> task = Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(_deletePassenger, conn))
                    {
                        cmd.Parameters.AddWithValue("@CarpoolId", carpool.CarpoolId);
                        cmd.Parameters.AddWithValue("@UserId", user.Id);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        conn.Close();
                        if (rowsAffected == 1)
                        {
                            return true;
                        } else
                        {
                            return false;
                            throw new InvalidOperationException("Der blev ikke slettet passager id: " + user.Id );
                        }
                    }
                }
            });

            return task;
        }

        public Task<Dictionary<Guid,Passenger>> GetPassengers (Carpool carpool)
        {
            Task<Dictionary<Guid,Passenger>> task = Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    Dictionary<Guid,Passenger> listOfpassenger = new Dictionary<Guid, Passenger>();

                    using (SqlCommand cmd = new SqlCommand(_getPassengers, conn))
                    {
                        cmd.Parameters.AddWithValue("@carpoolId", carpool.CarpoolId);
                        SqlDataReader reader = cmd.ExecuteReader();
                        
                        while (reader.Read())
                        {
                            Passenger passenger = new Passenger();
                            passenger.Carpool = new Carpool();
                            passenger.Carpool.CarpoolId = reader.GetInt32(0);
                            passenger.User = new User();
                            passenger.User.Id = reader.GetGuid(1);
                            passenger.IsAccepted = reader.GetBoolean(2);
                            listOfpassenger.Add(passenger.User.Id,passenger);
                        }
                        return listOfpassenger;
                        
                        
                    }
                }
            });
            return task;
        }

        public Task<bool> UpdatePassenger(Guid userId, int carpoolId)
        {
            Task<bool> task = Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(_updatePassenger, conn))
                    {
                        cmd.Parameters.AddWithValue("@CarpoolId", carpoolId);
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@request", true);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 1)
                        {
                            return true;
                        }
                        return false;
                        throw new InvalidOperationException("Der blev ikke updateret passager id: " +userId+ " carpool id: " + carpoolId);
                    }
                }
            });

            return task;
        }

        
        public Carpool MakeCarpool(SqlDataReader sqlReader)
        {
            Carpool carpool1 = new Carpool();
            carpool1.Driver = new User();
            carpool1.Driver.AddressList = new List<Address>();
            carpool1.Branch = new Branch();
            Address address1 = new Address();
            carpool1.Driver.Id = sqlReader.GetGuid(0);
            carpool1.Driver.Name = sqlReader.GetString(1);
            carpool1.Driver.Surname = sqlReader.GetString(2);
            carpool1.Driver.Email = sqlReader.GetString(3);
            carpool1.Driver.Phonenumber = sqlReader.GetString(4);
            
                address1.StreetName = sqlReader.GetString(5);
                address1.StreetNumber = sqlReader.GetString(6);
            if (sqlReader.GetDouble(7) != 0)
            {
                address1.Latitude = sqlReader.GetDouble(7);
                address1.Longtitude = sqlReader.GetDouble(8);
            }
                address1.CityName = sqlReader.GetString(9);
                address1.Postalcode = sqlReader.GetInt32(10);
                carpool1.Driver.AddressList.Add(address1);
            carpool1.CarpoolId = sqlReader.GetInt32(11);
            carpool1.Branch.BranchId = sqlReader.GetInt32(12);
            carpool1.PassengerSeats = sqlReader.GetInt32(13);
            carpool1.Date = sqlReader.GetDateTime(14);
            carpool1.Branch.BranchName = sqlReader.GetString(15);
            carpool1.Details = sqlReader.GetString(16);
            carpool1.Passengerlist = new Dictionary<Guid, Passenger>();
            return carpool1;
        }

        public Task<Dictionary<int,Carpool>> GetAllCarpools(DateTime date, string search)
        {
            Task<Dictionary<int, Carpool>> task = Task.Run(() =>
            {
                Carpool carpool = new Carpool();
                Dictionary<int, Carpool> carpools = new Dictionary<int, Carpool>();
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(_getAllCarpoolsSearch, conn))
                    {
                        cmd.Parameters.AddWithValue("@theDate", date);
                        cmd.Parameters.AddWithValue("@Search", search);
                        SqlDataReader reader = cmd.ExecuteReader();
                            
                        while (reader.Read())
                        {
                            carpool = MakeCarpool(reader);
                            carpools.Add(carpool.CarpoolId, carpool);
                        }
                        cmd.Dispose();
                        reader.Close();

                    }

                    using (SqlCommand cmd = new SqlCommand(_getAllPassengers, conn))
                    {
                        cmd.Parameters.AddWithValue("@theDate", date);
                        SqlDataReader reader = cmd.ExecuteReader();
                        
                        while (reader.Read())
                        {
                            Passenger passenger = new Passenger();
                            passenger.Carpool = new Carpool();
                            passenger.Carpool.CarpoolId = reader.GetInt32(0);
                            passenger.User = new User();
                            passenger.User.Id = reader.GetGuid(1);
                            passenger.IsAccepted = reader.GetBoolean(2);
                            if (carpools.ContainsKey(passenger.Carpool.CarpoolId))
                            {
                                carpools[passenger.Carpool.CarpoolId].Passengerlist.Add(passenger.User.Id, passenger);
                            }
                        }
                    }
                }


                return carpools;
            }); return task;
            
        }

        public Task<Dictionary<int, Carpool>> GetAllCarpools(Guid userId)
        {
            Task<Dictionary<int, Carpool>> task = Task.Run(() =>
            {
                Carpool carpool = new Carpool();
                Dictionary<int, Carpool> carpools = new Dictionary<int, Carpool>();
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(_getAllUserCarpools, conn))
                    {
                        
                        
                        SqlDataReader reader = cmd.ExecuteReader();
                        
                        while (reader.Read())
                        {
                            carpool = MakeCarpool(reader);
                            carpools.Add(carpool.CarpoolId, carpool);
                        }
                        cmd.Dispose();
                        reader.Close();

                    }

                    using (SqlCommand cmd = new SqlCommand(_getAllUserPassengers, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", userId);
                        SqlDataReader reader = cmd.ExecuteReader();
                        
                        while (reader.Read())
                        {
                            Passenger passenger = new Passenger();
                            passenger.Carpool = new Carpool();
                            passenger.Carpool.CarpoolId = reader.GetInt32(0);
                            passenger.User = new User();
                            passenger.User.Id = reader.GetGuid(1);
                            passenger.IsAccepted = reader.GetBoolean(2);
                            if (carpools.ContainsKey(passenger.Carpool.CarpoolId))
                            {
                                carpools[passenger.Carpool.CarpoolId].Passengerlist.Add(passenger.User.Id, passenger);
                            }
                        }
                    }
                }


                return carpools;
            }); return task;

        }



        public Task<Dictionary<int,Carpool>> GetAllCarpools(DateTime date)
        {
            Task<Dictionary<int,Carpool>> task = Task.Run(() =>
            {
                Carpool carpool = new Carpool();
                Dictionary<int,Carpool> carpools = new Dictionary<int, Carpool>();
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(_getAllCarpools, conn))
                    {
                        cmd.Parameters.AddWithValue("@theDate", date);
                        SqlDataReader reader = cmd.ExecuteReader();
                        
                        while (reader.Read())
                        {
                            carpool = MakeCarpool(reader);
                            carpools.Add(carpool.CarpoolId,carpool);
                        }
                        cmd.Dispose();
                        reader.Close();

                    }

                    using (SqlCommand cmd = new SqlCommand(_getAllPassengers, conn))
                    {
                        cmd.Parameters.AddWithValue("@theDate", date);
                        SqlDataReader reader = cmd.ExecuteReader();
                        
                        while (reader.Read())
                        {
                            Passenger passenger = new Passenger();
                            passenger.Carpool = new Carpool();
                            passenger.Carpool.CarpoolId = reader.GetInt32(0);
                            passenger.User = new User();
                            passenger.User.Id = reader.GetGuid(1);
                            passenger.IsAccepted = reader.GetBoolean(2);
                            if (carpools.ContainsKey(passenger.Carpool.CarpoolId))
                            {
                                carpools[passenger.Carpool.CarpoolId].Passengerlist.Add(passenger.User.Id,passenger);
                            }
                        }
                    }
                }
                
                
                return carpools;
            }); return task;
        }

       
    }
}