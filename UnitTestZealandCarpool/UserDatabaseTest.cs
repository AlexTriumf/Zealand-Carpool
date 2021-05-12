using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Zealand_Carpool.Models;
using Zealand_Carpool.Services;
namespace UnitTestZealandCarpool
{
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
                User user1 = new UserDatabaseAsync().GetUserID(user.Email, "12345678").Result;
            user.Id = user1.Id;
            return user;
        }

        //[TestMethod]
        //public void UserAddToDatabaseTest()
        //{
        //    //Arrange
        //    User user = new User();
        //    user.Name = "Test";
        //    user.Phonenumber = "+45 000000";
        //    user.Surname = "Test";
        //    user.UserType = UserType.Admin;
        //    user.Email = "test@email.com";
        //    List<Address> addresses = new List<Address>();
        //    Address address = new Address("Maglegårdsvej", "2", 4000, "Roskilde");
        //    address.Latitude = 55.6307194;
        //    address.Longtitude = 12.0786884;
        //    addresses.Add(address);
        //    user.AddressList = addresses;
        //    user.Password = "12345678";
        //    //Act & Assert
        //    Assert.IsTrue(new UserDatabaseAsync().AddUser(user).Result);
        //}

        [TestMethod]
        public void UserGetFromDatabaseTest()
        {
            //Arrange
            User user = GetUser();
            string password = "12345678";
            User newUser = new UserDatabaseAsync().GetUser(user.Email, password).Result;

            //Act & Assert
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
            Assert.AreEqual(new UserDatabaseAsync().GetUserID(user.Email, password).Result.Id, user.Id);
        }

        [TestMethod]
        public void UserUpdateFromDatabaseTest()
        {
            //Arrange
            User user = GetUser();
            user.Name = "testUpdate";
            string password = "12345678";
            User newUser = new UserDatabaseAsync().GetUser(user.Email, password).Result;

            //Act & Assert
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

        //[TestMethod]
        //public void UserRemoveFromDatabaseTest()
        //{
        //    //Arrange
        //    User user = GetUser();
        //    //Act & Assert
        //    Assert.IsTrue(new UserDatabaseAsync().DeleteUser(user.Id).Result);
        //}

        [TestMethod]
        public void UserSearchFromDatabaseTest()
        {
            //Arrange
            string search = "testUpdate";
            User user = GetUser();
            //Act & Assert
            Assert.AreEqual(new UserDatabaseAsync().SearchUsers(search)[0].Name,user.Name);
        }




    }
}
