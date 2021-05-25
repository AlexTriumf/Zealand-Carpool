using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Zealand_Carpool.Models;
using Zealand_Carpool.Services;
namespace UnitTestZealandCarpool
{
    /// <summary>
    /// A test for UserPersistenceAsync class
    /// This test can¨t run all test at ones, the user has to be added first.
    /// Made by Andreas
    /// </summary>
    [TestClass]
    public class UserDatabaseTest
    {
        public User GetUser()
        {
            User user = new User();
            user.Name = "Test";
            user.Phonenumber = "+45 000000";
            user.Surname = "Test";
            user.UserType = UserType.Admin;
            user.Email = "test@email.com";
            List<Address> addresses = new List<Address>();
            Address address = new Address("Maglegårdsvej", "2", 4000, "Roskilde");
            address.Latitude = 55.6307194;
            address.Longtitude = 12.0786884;
            addresses.Add(address);
            user.AddressList = addresses;
                User user1 = new UserPersistenceAsync().GetUserID(user.Email, "12345678").Result;
            user.Id = user1.Id;
            return user;
        }

        [TestMethod]
        public void UserAddToDatabaseTest()
        {
            //Arrange
            User user = new User();
            user.Name = "Test";
            user.Phonenumber = "+45 000000";
            user.Surname = "Test";
            user.UserType = UserType.Admin;
            user.Email = "test@email.com";
            List<Address> addresses = new List<Address>();
            Address address = new Address("Maglegårdsvej", "2", 4000, "Roskilde");
            address.Latitude = 55.6307194;
            address.Longtitude = 12.0786884;
            addresses.Add(address);
            user.AddressList = addresses;
            user.Password = "12345678";
            //Act & Assert
            Assert.IsTrue(new UserPersistenceAsync().AddUser(user).Result);
        }

        [TestMethod]
        public void UserGetFromDatabaseTest()
        {
            //Arrange
            User user = GetUser();
            string password = "12345678";
            
            //Act
            User newUser = new UserPersistenceAsync().GetUser(user.Email, password).Result;

            //Assert
            Assert.AreEqual(newUser.Id,user.Id);
            Assert.AreEqual(newUser.Name, user.Name);
            Assert.AreEqual(newUser.Phonenumber, user.Phonenumber);
            Assert.AreEqual(newUser.Surname, user.Surname);
            Assert.AreEqual(newUser.UserType, user.UserType);
            Assert.AreEqual(newUser.AddressList[0].CityName, user.AddressList[0].CityName);
            Assert.AreEqual(newUser.AddressList[0].Latitude, user.AddressList[0].Latitude);
            Assert.AreEqual(newUser.AddressList[0].Longtitude, user.AddressList[0].Longtitude);
            Assert.AreEqual(newUser.AddressList[0].Postalcode, user.AddressList[0].Postalcode);
            Assert.AreEqual(newUser.AddressList[0].StreetName, user.AddressList[0].StreetName);
            Assert.AreEqual(newUser.AddressList[0].StreetNumber, user.AddressList[0].StreetNumber);
            Assert.AreEqual(newUser.Email, user.Email);
        }
        [TestMethod]
        public void UserIDGetFromDatabaseTest()
        {
            //Arrange
            User user = GetUser();
            string password = "12345678";
            //Act & Assert
            Assert.AreEqual(new UserPersistenceAsync().GetUserID(user.Email, password).Result.Id, user.Id);
        }

        [TestMethod]
        public void UserUpdateFromDatabaseTest()
        {
            //Arrange
            User user = GetUser();
            user.Name = "testUpdate";

            user.Password= "12345678";
            //Act
            bool isUpdated = new UserPersistenceAsync().UpdateUser(user.Id, user).Result;

            User newUser = new UserPersistenceAsync().GetUser(user.Email, user.Password).Result;
            //Assert
            Assert.AreEqual(newUser.Id, user.Id);
            Assert.AreEqual(newUser.Name, user.Name);
            Assert.AreEqual(newUser.Phonenumber, user.Phonenumber);
            Assert.AreEqual(newUser.Surname, user.Surname);
            Assert.AreEqual(newUser.UserType, user.UserType);
            Assert.AreEqual(newUser.AddressList[0].CityName, user.AddressList[0].CityName);
            Assert.AreEqual(newUser.AddressList[0].Latitude, user.AddressList[0].Latitude);
            Assert.AreEqual(newUser.AddressList[0].Longtitude, user.AddressList[0].Longtitude);
            Assert.AreEqual(newUser.AddressList[0].Postalcode, user.AddressList[0].Postalcode);
            Assert.AreEqual(newUser.AddressList[0].StreetName, user.AddressList[0].StreetName);
            Assert.AreEqual(newUser.AddressList[0].StreetNumber, user.AddressList[0].StreetNumber);
            Assert.AreEqual(newUser.Email, user.Email);
        }

        [TestMethod]
        public void UserRemoveFromDatabaseTest()
        {
            //Arrange
            User user = GetUser();
            //Act & Assert
            Assert.IsTrue(new UserPersistenceAsync().DeleteUser(user.Id).Result);
        }

        [TestMethod]
        public void UserSearchFromDatabaseTest()
        {
            //Arrange
            string search = "Test";
           
            //Act & Assert
            Assert.AreEqual(new UserPersistenceAsync().SearchUsers(search)[0].Name,search);
        }

        [TestMethod]
        public void UsercheckFromDatabaseTest()
        {
            //Arrange
            User user = GetUser();
            user.Password = "12345678";
            //Act & Assert
            Assert.IsFalse(new UserPersistenceAsync().CheckUser(user).Result) ;
        }

        [TestMethod]
        public void UserAddToDatabaseTestFail()
        {
            //Arrange
            User user = new User();
            
            //Act & Assert
            Assert.ThrowsException<AggregateException>(() => new UserPersistenceAsync().AddUser(user).Result);
        }

        [TestMethod]
        public void UserUpdateFromDatabaseTestFail()
        {
            //Arrange
            User user = new User();

            //Act & Assert
            Assert.ThrowsException<AggregateException>(() => new UserPersistenceAsync().UpdateUser(user.Id,user).Result);
        }

        [TestMethod]
        public void UserRemoveFromDatabaseTestFail()
        {
            //Arrange
            User user = new User();
            //Act & Assert
            Assert.IsFalse(new UserPersistenceAsync().DeleteUser(Guid.NewGuid()).Result);
        }

        [TestMethod]
        public void UserGetListsFromDatabaseTest()
        {
            //Arrange
            User user = GetUser();
            //Act & Assert
            Assert.IsNotNull(new UserPersistenceAsync().GetAllPostalCodes().Result);
            Assert.IsNotNull(new UserPersistenceAsync().GetAllUsers().Result);
          
        }


    }
}
