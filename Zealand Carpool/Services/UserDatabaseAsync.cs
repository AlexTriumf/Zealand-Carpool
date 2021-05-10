﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;
using Zealand_Carpool.Models.Json;

namespace Zealand_Carpool.Services
{
    /// <summary>
    /// A UserDatebase connection class which implements IUser.
    /// Made by Andreas
    /// </summary>
    public class UserDatabaseAsync : IUser
    {
        string _connString = "Data Source=andreas-zealand-server-dk.database.windows.net;Initial Catalog=Andreas-database;User ID=Andreas;Password=SecretPassword!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        string _createUser = "insert into UserTable (Name, Surname, " +
                            "Email, Phonenumber, UserType, Password) Values (@Name, @Surname, @Email, @Phonenumber, @UserType, @Password)";

        string _createUserToAddress = "insert into AddressList (UserId, StreetName, " +
                            "Streetnr, PostalCode, Latitude, Longtitude) Values (@id, @streetname, @streetnumber, @PostalCode, @lat, @long)";

        string _getUser = "SELECT UserTable.UserId,UserTable.Name,UserTable.Surname,UserTable.Email,UserTable.Phonenumber,UserTable.UserType,AddressList.StreetName,AddressList.Streetnr,AddressList.Latitude,AddressList.Longtitude,PostalCode.City,PostalCode.PostalCode FROM UserTable " +
                                    "INNER JOIN AddressList ON UserTable.UserId=AddressList.UserId INNER join PostalCode on AddressList.PostalCode=PostalCode.PostalCode Where UserTable.UserId = @id;";

        string _getUserById = "SELECT * From UserTable WHERE UserTable.Email = @email and UserTable.Password = @password";

        string _getUserFEP = "SELECT UserTable.UserId,UserTable.Name,UserTable.Surname,UserTable.Email,UserTable.Phonenumber,UserTable.UserType,AddressList.StreetName,AddressList.Streetnr,AddressList.Latitude,AddressList.Longtitude,PostalCode.City,PostalCode.PostalCode FROM UserTable " +
                                    "INNER JOIN AddressList ON UserTable.UserId=AddressList.UserId INNER join PostalCode on AddressList.PostalCode=PostalCode.PostalCode " +
                                    " WHERE UserTable.Email = @email and UserTable.Password = @password";

        string _deleteUser = "delete from UserTable where UserId = @ID";

        string _updateUser = "Update UserTable set Name=@name, Surname=@surname, Email=@email, Phonenumber=@phonenumber, UserType=usertype, Password=@password where UserId = @id";

        string _updateUserAddress = "Update AddressList set StreetName=@streetname, Streetnr=@streetnr, PostalCode=@postalcode, Latitude=@lat, Longtitude=@long where UserId = @id";

        string _getAllUsers = "SELECT UserTable.UserId,UserTable.Name,UserTable.Surname,UserTable.Email,UserTable.Phonenumber,UserTable.UserType,AddressList.StreetName,AddressList.Streetnr,AddressList.Latitude,AddressList.Longtitude,PostalCode.City,PostalCode.PostalCode FROM UserTable " +
                                    "INNER JOIN AddressList ON UserTable.UserId=AddressList.UserId INNER join PostalCode on AddressList.PostalCode=PostalCode.PostalCode";

        string _getAllPostalCodes = "SELECT * FROM PostalCode";
        
        public Task<bool> AddUser(User user)
        {

            Task task = Task.Run(async () => {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(_createUser, conn))
                    {

                        cmd.Parameters.AddWithValue("@Name", user.Name);
                        cmd.Parameters.AddWithValue("@Surname", user.Surname);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@Phonenumber", user.Phonenumber);
                        cmd.Parameters.AddWithValue("@UserType", user.UserType);
                        cmd.Parameters.AddWithValue("@Password", user.Password);
                        cmd.ExecuteNonQuery();
                    }

                    Task<User> task1 = GetUserID(user.Email, user.Password);
                    task1.Wait();
                    user.Id = task1.Result.Id;
                    using var client = new HttpClient();
                    var content = await client.GetStringAsync("https://maps.googleapis.com/maps/api/geocode/json?address=" + user.AddressList[0].StreetName + "+" + user.AddressList[0].StreetNumber + "+" + user.AddressList[0].Postalcode + "&key=AIzaSyC2t8TFM7VJY_gUpk45PYxbxqqxPcasVho");
                    Geo geoData = JsonConvert.DeserializeObject<Geo>(content);

                    user.AddressList[0].Latitude = double.Parse(geoData.results[0].geometry.location.lat, new CultureInfo("en-UK"));
                    user.AddressList[0].Longtitude = double.Parse(geoData.results[0].geometry.location.lng, new CultureInfo("en-UK"));
                    using (SqlCommand cmd = new SqlCommand(_createUserToAddress, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", user.Id);
                        cmd.Parameters.AddWithValue("@streetname", user.AddressList[0].StreetName);
                        cmd.Parameters.AddWithValue("@streetnumber", user.AddressList[0].StreetNumber);
                        cmd.Parameters.AddWithValue("@PostalCode", user.AddressList[0].Postalcode);
                        cmd.Parameters.AddWithValue("@lat", user.AddressList[0].Latitude);
                        cmd.Parameters.AddWithValue("@long", user.AddressList[0].Longtitude);
                        cmd.ExecuteNonQuery();
                    }
                }
            });
           
            return Task.FromResult(task.IsCompletedSuccessfully);
        }

       

        public Task<bool> DeleteUser(Guid id)
        {

            Task<bool> task = Task.Run(() => {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(_deleteUser, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0) return true;
                    }
                    
                }
                return false;
                
            });
            
            return task;
        }

        public Task<User> GetUser(Guid id)
        {
            User user = new User();
            Task<User> task = Task.Run(() => {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(_getUser, conn))
                    {
                        User user = new User();
                        cmd.Parameters.AddWithValue("@id", id);
                        SqlDataReader reader = cmd.ExecuteReader();
                        reader.Read();
                        user = MakeUser(reader);

                        return user;
                    }
                }
            });
            
            return task;
        }

        public Task<User> GetUserID(string email, string password)
        {
            Task<User> task = Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(_getUserById, conn))
                    {

                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", password);
                        SqlDataReader reader = cmd.ExecuteReader();
                        User user = new User();
                        
                        while (reader.Read())
                        {
                        user.Id = reader.GetGuid(0);
                        }
                        return user;
                    }
                }
            });

            return task;
        }
        public Task<User> GetUser(string email, string password)
        {
            
            Task<User> task = Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(_getUserFEP, conn))
                    {
                        
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", password);
                        SqlDataReader reader = cmd.ExecuteReader();
                        reader.Read();
                        if (!reader.HasRows)
                        {
                            return null;
                        }
                        User user = MakeUser(reader);
                        return user;
                    }
                }
            });
            
            return task;
        }

        public User MakeUser(SqlDataReader sqlReader)
        {
            User user = new User();
            Address address = new Address();
            user.AddressList = new List<Address>();
            
            
                user.Id = sqlReader.GetGuid(0);
                user.Name = sqlReader.GetString(1);
                user.Surname = sqlReader.GetString(2);
                user.Email = sqlReader.GetString(3);
                user.Phonenumber = sqlReader.GetString(4);
                user.UserType = (UserType) sqlReader.GetInt32(5);
                if (sqlReader.GetString(6) != null)
                {
                address.StreetName = sqlReader.GetString(6);
                address.StreetNumber = sqlReader.GetString(7);
                address.Latitude = sqlReader.GetDouble(8);
                address.Longtitude = sqlReader.GetDouble(9);
                address.CityName = sqlReader.GetString(10);
                address.Postalcode = sqlReader.GetInt32(11);
                user.AddressList.Add(address);
                }
            
            return user;
        }

        public Task<bool> UpdateUser(Guid id, User user)
        {
            Task task = Task.Run(async () =>
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(_updateUser, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@name", user.Name);
                        cmd.Parameters.AddWithValue("@surname", user.Surname);
                        cmd.Parameters.AddWithValue("@email", user.Email);
                        cmd.Parameters.AddWithValue("@phonenumber", user.Phonenumber);
                        cmd.Parameters.AddWithValue("@usertype", user.UserType);
                        cmd.Parameters.AddWithValue("@password", user.Password);

                        cmd.ExecuteNonQuery();

                    }
                    using var client = new HttpClient();
                    var content = await client.GetStringAsync("https://maps.googleapis.com/maps/api/geocode/json?address=" + user.AddressList[0].StreetName + "+" + user.AddressList[0].StreetNumber + "+" + user.AddressList[0].Postalcode + "&key=AIzaSyC2t8TFM7VJY_gUpk45PYxbxqqxPcasVho");
                    Geo geoData = JsonConvert.DeserializeObject<Geo>(content);

                    user.AddressList[0].Latitude = double.Parse(geoData.results[0].geometry.location.lat, new CultureInfo("en-UK"));
                    user.AddressList[0].Longtitude = double.Parse(geoData.results[0].geometry.location.lng, new CultureInfo("en-UK"));
                    using (SqlCommand cmd = new SqlCommand(_updateUserAddress, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", user.Id);
                        cmd.Parameters.AddWithValue("@streetname", user.AddressList[0].StreetName);
                        cmd.Parameters.AddWithValue("@streetnr", user.AddressList[0].StreetNumber);
                        cmd.Parameters.AddWithValue("@postalcode", user.AddressList[0].Postalcode);
                        cmd.Parameters.AddWithValue("@lat", user.AddressList[0].Latitude);
                        cmd.Parameters.AddWithValue("@long", user.AddressList[0].Longtitude);
                        cmd.ExecuteNonQuery();
                    }
                }
            });
            return Task.FromResult(task.IsCompletedSuccessfully);
        }

        public Task<Dictionary<Guid,User>> GetAllUsers()
        {
            Task<Dictionary<Guid, User>> task = Task.Run(() =>
            {
                Dictionary<Guid, User> dicOfUsers = new Dictionary<Guid, User>();
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(_getAllUsers, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            User user = MakeUser(reader);
                            dicOfUsers.Add(user.Id, user);

                        }
                        return dicOfUsers;
                        
                    }
                }
            });

            return task;
        }
<<<<<<< HEAD
=======

        public Task<List<Branch>> GetAllPostalCodes()
        {
            Task<List<Branch>> task = Task.Run(() =>
            {
                List<Branch> listOfCodes = new List<Branch>();
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(_getAllPostalCodes, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Branch branch = new Branch();
                            branch.BranchPostalCode = reader.GetInt32(0);
                            branch.BranchName = reader.GetString(1);
                            listOfCodes.Add(branch);
                        }
                        return listOfCodes;
                    }
                }
            });

            return task;
        }
>>>>>>> master
        //Written by Malte
        //Primitiv søgefunktion for flere ord
        private string NameSplitter(string name) {
            string input = name;
            string[] names = input.Split(' ');
            string firstName = names[0];
            if (names.Length == 1)
            {
                return "SELECT * FROM UserTable WHERE Name LIKE '%" + firstName + "%' OR Surname LIKE '%" + firstName +"%';";
            }

            if (names.Length > 1)
            {
                string surName = names[1];
                return "SELECT * FROM UserTable WHERE Name LIKE (" + firstName + " OR " + surName +
                       ") OR Surname LIKE (" + firstName + " OR" + surName + ");";
            }
            else throw new Exception("Wrong input");


        }
        public List<User> SearchUsers(string name)
        {
            List<User> userList = new List<User>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(NameSplitter(name), conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        User user = new User();
                        user.Id = reader.GetGuid(0);
                        userList.Add(GetUser(user.Id).Result);
                    }
                }
                conn.Close();
            }
            
            return userList;
        }
    }
}
