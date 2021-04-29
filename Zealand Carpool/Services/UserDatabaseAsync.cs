using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Services
{
    /// <summary>
    /// A UserDatebase connection class which implements IUser.
    /// Made by Andreas
    /// </summary>
    public class UserDatabaseAsync : IUser
    {
        string ConnString = "Data Source=andreas-zealand-server-dk.database.windows.net;Initial Catalog=Andreas-database;User ID=Andreas;Password=SecretPassword!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        string createUser = "insert into UserTable (Name, Surname, " +
                            "Email, Phonenumber, UserType, Password) Values (@Name, @Surname, @Email, @Phonenumber, @UserType, @Password)";

        string createUserToAddress = "insert into AddressList (UserId, StreetName, " +
                            "Streetnr, PostalCode) Values (@id, @streetname, @streetnumber, @PostalCode)";

        string getUser = "SELECT UserTable.UserId,UserTable.Name,UserTable.Surname,UserTable.Email,UserTable.Phonenumber,UserTable.UserType,AddressList.StreetName,AddressList.Streetnr,PostalCode.City,PostalCode.PostalCode FROM UserTable " +
                                    "INNER JOIN AddressList ON UserTable.UserId=AddressList.UserId INNER join PostalCode on AddressList.PostalCode=PostalCode.PostalCode Where UserTable.UserId = @id;";

        string getUserById = "SELECT * From UserTable WHERE UserTable.Email = @email and UserTable.Password = @password";

        string getUserFEP = "SELECT UserTable.UserId,UserTable.Name,UserTable.Surname,UserTable.Email,UserTable.Phonenumber,UserTable.UserType,AddressList.StreetName,AddressList.Streetnr,PostalCode.City,PostalCode.PostalCode FROM UserTable " +
                                    "INNER JOIN AddressList ON UserTable.UserId=AddressList.UserId INNER join PostalCode on AddressList.PostalCode=PostalCode.PostalCode " +
                                    " WHERE UserTable.Email = @email and UserTable.Password = @password";

        string deleteUser = "delete from UserTable where UserId = @ID";

        string updateUser = "Update UserTable set Name=@name, Surname=@surname, Email=@email, Phonenumber=@phonenumber, UserType=usertype, Password=@password where UserId = @id";

        public Task<bool> AddUser(User user)
        {

            Task task = Task.Run(() => {
                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(createUser, conn))
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
                    using (SqlCommand cmd = new SqlCommand(createUserToAddress, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", user.Id);
                        cmd.Parameters.AddWithValue("@streetname", user.AddressList[0].StreetName);
                        cmd.Parameters.AddWithValue("@streetnumber", user.AddressList[0].StreetNumber);
                        cmd.Parameters.AddWithValue("@PostalCode", user.AddressList[0].Postalcode);
                        cmd.ExecuteNonQuery();
                    }
                }
            });
           
            return Task.FromResult(task.IsCompletedSuccessfully);
        }

       

        public Task<bool> DeleteUser(Guid id)
        {

            Task task = new Task(() => {
                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(deleteUser, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);

                        cmd.ExecuteNonQuery();
                    }
                }
            });
            
            return Task.FromResult(task.IsCompletedSuccessfully);
        }

        public Task<User> GetUser(Guid id)
        {
            
            User user = new User();
            Task<User> task = Task.Run(() => {
                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(getUser, conn))
                    {
                        User user = new User();
                        cmd.Parameters.AddWithValue("@id", id);
                        SqlDataReader reader = cmd.ExecuteReader();
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
                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(getUserById, conn))
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
                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(getUserFEP, conn))
                    {
                        
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", password);
                        SqlDataReader reader = cmd.ExecuteReader();
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
            while (sqlReader.Read())
            {
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
                address.CityName = sqlReader.GetString(8);
                address.Postalcode = sqlReader.GetInt32(9);
                user.AddressList.Add(address);
                }
            }
            return user;
        }

        public Task<bool> UpdateUser(Guid id, User user)
        {
            Task task = new Task(() =>
            {
                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(updateUser, conn))
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
                }
            });
            return Task.FromResult(task.IsCompletedSuccessfully);
        }
    }
}
