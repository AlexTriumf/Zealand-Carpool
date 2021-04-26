using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Services
{
    public class UserDatabase : IUser
    {
        SqlConnection databaseCon = new DatabaseCon().GetConnection();
        string createUser = "insert into UserTable (Name, Surname, " +
                            "Email, Phonenumber, UserType, Password) Values (@Name, @Surname, @Email, @Phonenumber, @UserType, @Password)";

        string createUserToAddress = "insert into AddressList (UserId, StreetName, " +
                            "Streetnr, PostalCode) Values (@id, @streetname, @streetnumber, @PostalCode)";
        
        string getUser = "SELECT UserTable.UserId,UserTable.Name,UserTable.Surname,UserTable.Email,UserTable.Phonenumber,UserTable.UserType,UserTable.Password,AddressList.StreetName,AddressList.Streetnr,PostalCode.City,PostalCode.PostalCode FROM UserTable "+
                                    "INNER JOIN AddressList ON UserTable.UserId=AddressList.UserId INNER join PostalCode on AddressList.PostalCode=PostalCode.PostalCode Where UserTable.UserId = @id;";

        string getUserFEP = "SELECT UserTable.UserId,UserTable.Name,UserTable.Surname,UserTable.Email,UserTable.Phonenumber,UserTable.UserType,UserTable.Password,AddressList.StreetName,AddressList.Streetnr,PostalCode.City,PostalCode.PostalCode FROM UserTable " +
                                    "INNER JOIN AddressList ON UserTable.UserId=AddressList.UserId INNER join PostalCode on AddressList.PostalCode=PostalCode.PostalCode Where UserTable.UserId = @id" +
                                    " WHERE UserTable.Email = @email and UserTable.Password = @password";

        string deleteUser = "delete from Users where UserId = @ID";

        public Task<bool> AddUser(User user)
        {
            
            Task task = new Task(() => { 
            using (SqlCommand cmd = new SqlCommand(createUser, databaseCon))
            {

                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Surname", user.Surname);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Phonenumber", user.Phonenumber);
                cmd.Parameters.AddWithValue("@UserType", user.UserType);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                
                cmd.ExecuteNonQuery();
            
            }
                Task<User> user1 = GetUser(user.Email, user.Password);
                user1.Wait();
                if (user1.IsCompleted)
                {
                    using (SqlCommand cmd = new SqlCommand(createUserToAddress, databaseCon))
                    {
                        cmd.Parameters.AddWithValue("@id", user.Name);
                        cmd.Parameters.AddWithValue("@streetname", user.Surname);
                        cmd.Parameters.AddWithValue("@streetnumber", user.Email);
                        cmd.Parameters.AddWithValue("@PostalCode", user.Phonenumber);
                        cmd.ExecuteNonQuery();
                    }
                }
            });
            databaseCon.Close();
            return Task.FromResult(task.IsCompletedSuccessfully);
        }

        public Task<bool> DeleteUser(Guid id)
        {
           
            Task task = new Task(() => {
                using (SqlCommand cmd = new SqlCommand(deleteUser, databaseCon))
                {
                    cmd.Parameters.AddWithValue("@Name", id);
                   
                    cmd.ExecuteNonQuery();
                }
            });
            databaseCon.Close();
            return Task.FromResult(task.IsCompletedSuccessfully);
        }

        public Task<User> GetUser(Guid id)
        {
            
            User user = new User();
            Task<User> task = Task.Run(() => {
                using (SqlCommand cmd = new SqlCommand(getUser, databaseCon))
                {
                    User user = new User();
                    SqlDataReader reader = cmd.ExecuteReader();
                    cmd.Parameters.AddWithValue("@id", id);
                    user = MakeUser(reader);
                    
                    return user;
                }
            });
            databaseCon.Close();
            return task;
        }

        public Task<User> GetUser(string email, string password)
        {
            
            Task<User> task = Task.Run(() => {
                using (SqlCommand cmd = new SqlCommand(getUserFEP, databaseCon))
                {
                    User user = new User();
                    SqlDataReader reader = cmd.ExecuteReader();
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@email", password);
                    user = MakeUser(reader);
                    return user;
                }
            });
            databaseCon.Close();
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
            user.UserType = (UserType) sqlReader.GetInt32(4);
            address.StreetName = sqlReader.GetString(5);
            address.StreetNumber = sqlReader.GetInt32(6);
            address.Postalcode = sqlReader.GetInt32(7);
            address.CityName = sqlReader.GetString(8);
            user.AddressList.Add(address);
            }
            return user;
        }

        public Task<bool> UpdateUser(Guid id, User user)
        {
            throw new NotImplementedException();
        }
    }
}
