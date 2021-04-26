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

        string createUser = "insert into UserTable (Name, Surname, " +
                            "Email, Phonenumber, UserType, Password) Values (@Name, @Surname, @Email, @Phonenumber, @UserType, @Password)";

        string deleteUser = "delete from Users where UserId = @ID";

        public Task<bool> AddUser(User user)
        {
            SqlConnection databaseCon = new DatabaseCon().GetConnection();
            Task task = new Task(() => { 
            using (SqlCommand cmd = new SqlCommand(createUser, databaseCon))
            {

                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Surname", user.Surname);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Phonenumber", user.Phonenumber);
                cmd.Parameters.AddWithValue("@UserType", user.UserType);
                cmd.Parameters.AddWithValue("@Password", user.Phonenumber);
                
                cmd.ExecuteNonQuery();
            
            }});
            databaseCon.Close();
            return Task.FromResult(task.IsCompletedSuccessfully);
        }

        public Task<bool> DeleteUser(Guid id)
        {
            SqlConnection databaseCon = new DatabaseCon().GetConnection();
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
            
        }

        public Task<User> GetUser(string email, string password)
        {
            throw new NotImplementedException();
        }

        public User MakeUser(SqlDataReader sqlReader)
        {
            User user = new User();
            Address address = new Address();
            user.Id = sqlReader.GetGuid(0);
            user.Name = sqlReader.GetString(1);
            user.Surname = sqlReader.GetString(2);
            user.Email = sqlReader.GetString(3);
            user.UserType = (UserType) sqlReader.GetInt32(4);
            address.
            user.AddressList = new List<Address>();
            user.AddressList.Add(address);
        }

        public Task<bool> UpdateUser(Guid id, User user)
        {
            throw new NotImplementedException();
        }
    }
}
