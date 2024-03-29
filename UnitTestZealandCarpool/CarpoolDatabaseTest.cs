using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Zealand_Carpool.Models;
using Zealand_Carpool.Services;

namespace UnitTestZealandCarpool
{
    /// <summary>
    /// A test for PersistenceAsync class
    /// This test can�t run all test at ones, the user has to be added first.
    /// Made by Andreas
    /// </summary>
    [TestClass]
    public class CarpoolDatabaseTest
    {
        public Carpool GetCarpool()
        {
           
            DateTime time = DateTime.Parse("13-05-2021 11:43:32");
            Carpool carpool = new Carpool();
            carpool.Branch = new Branch();
            carpool.Branch.BranchId = 1;
            carpool.Branch.BranchName = "Zealand Roskilde";
            carpool.Branch.BranchPostalCode = 4000;

            
            carpool.Driver = new UserDatabaseTest().GetUser();
            carpool.Passengerlist = new Dictionary<System.Guid, Passenger>();
           
            carpool.Date = time;
            carpool.Details = "Dette er detaljer";
            carpool.PassengerSeats = 3;
            return carpool;
        }

        [TestMethod]
        public void CarpoolAddToDatabaseTest()
        {
            //Arrange
            Carpool carpool = GetCarpool();
            //Act & Assert
            Assert.IsTrue(new CarpoolPersistenceAsync().AddCarpool(carpool).Result);
            Assert.ThrowsException<AggregateException>(() => new CarpoolPersistenceAsync().AddCarpool(new Carpool()).Result);
        }

        [TestMethod]
        public void CarpoolGetFromDatabaseTest()
        {
            //Arrange
            Carpool carpool = GetCarpool();
            List<Carpool> carpools = new List<Carpool>();
            foreach (Carpool carpool1 in new CarpoolPersistenceAsync().GetAllCarpools(carpool.Driver.Id).Result.Values)
            {
                if (carpool.Driver.Id == carpool1.Driver.Id)
                {
                    carpool.CarpoolId = carpool1.CarpoolId; 
                    carpools.Add(carpool1);
                }
            }

            Carpool carpool2 = new CarpoolPersistenceAsync().GetCarpool(carpools[0].CarpoolId).Result;
            //Act & Assert
            Assert.AreEqual(carpool.CarpoolId, new CarpoolPersistenceAsync().GetCarpool(carpools[0].CarpoolId).Result.CarpoolId);
            //Assert.AreEqual(carpool.Date,new CarpoolDatabaseAsync().GetCarpool(carpools[0].CarpoolId).Result.Date); is the same value 
            Assert.AreEqual(carpool.Details, new CarpoolPersistenceAsync().GetCarpool(carpools[0].CarpoolId).Result.Details);
            Assert.AreEqual(carpool.Driver.Id, new CarpoolPersistenceAsync().GetCarpool(carpools[0].CarpoolId).Result.Driver.Id);
            Assert.AreEqual(carpool.PassengerSeats, new CarpoolPersistenceAsync().GetCarpool(carpools[0].CarpoolId).Result.PassengerSeats);
            Assert.AreEqual(carpool.Branch.BranchId, new CarpoolPersistenceAsync().GetCarpool(carpools[0].CarpoolId).Result.Branch.BranchId);
            Assert.AreEqual(carpool.Branch.BranchName, new CarpoolPersistenceAsync().GetCarpool(carpools[0].CarpoolId).Result.Branch.BranchName);
            
            Assert.IsNotNull(new CarpoolPersistenceAsync().GetCarpool(100000));

        }


        [TestMethod]
        public void CarpoolDeleteFromDatabaseTest()
        {
            //Arrange
            Carpool carpool = GetCarpool();

            foreach (Carpool carpool1 in new CarpoolPersistenceAsync().GetAllCarpools(carpool.Driver.Id).Result.Values)
            {
                if (carpool.Driver.Id == carpool1.Driver.Id)
                {
                    carpool.CarpoolId = carpool1.CarpoolId;

                }
            }

            //Act & Assert
            Assert.IsTrue(new CarpoolPersistenceAsync().DeleteCarpool(carpool.CarpoolId).Result);
        }

        [TestMethod]
        public void CarpoolGetBranchesFromDatabaseTest()
        {
            //Arrange
            Branch branch = new Branch();

            //Act & Assert
            Assert.IsNotNull(new CarpoolPersistenceAsync().GetBranches().Result);
        }

        [TestMethod]
        public void CarpoolAddPassengerToDatabaseTest()
        {
            //Arrange
            Carpool carpool = GetCarpool();
            foreach (Carpool carpool1 in new CarpoolPersistenceAsync().GetAllCarpools(carpool.Driver.Id).Result.Values)
            {
                if (carpool.Driver.Id == carpool1.Driver.Id)
                {
                    carpool.CarpoolId = carpool1.CarpoolId;
                    
                }
            }

            //Act & Assert needs a carpool to be added
            Assert.IsTrue(new CarpoolPersistenceAsync().AddPassenger(new UserDatabaseTest().GetUser(),carpool).Result);
   
            Assert.ThrowsException<AggregateException>(() => new CarpoolPersistenceAsync().AddPassenger(new User(),new Carpool()).Result);
        }

        [TestMethod]
        public void CarpoolDeletePassengerToDatabaseTest()
        {
            //Arrange
            Carpool carpool = GetCarpool();
            foreach (Carpool carpool1 in new CarpoolPersistenceAsync().GetAllCarpools(carpool.Driver.Id).Result.Values)
            {
                if (carpool.Driver.Id == carpool1.Driver.Id)
                {
                    carpool.CarpoolId = carpool1.CarpoolId;

                }
            }

            //Act & Assert needs a carpool to be added
            Assert.IsTrue(new CarpoolPersistenceAsync().DeletePassenger(carpool.Driver,carpool).Result);

            
        }

        [TestMethod]
        public void CarpoolgetPassengersFromDatabaseTest()
        {
            //Arrange
            Carpool carpool = GetCarpool();
            foreach (Carpool carpool1 in new CarpoolPersistenceAsync().GetAllCarpools(carpool.Driver.Id).Result.Values)
            {
                if (carpool.Driver.Id == carpool1.Driver.Id)
                {
                    carpool.CarpoolId = carpool1.CarpoolId;

                }
            }

            //Act & Assert needs a carpool to be added
            Assert.AreEqual(carpool.Driver.Id, new CarpoolPersistenceAsync().GetPassengers(carpool).Result[carpool.Driver.Id].User.Id);
            
        }

        [TestMethod]
        public void CarpoolUpdatePassengersFromDatabaseTest()
        {
            //Arrange
            Carpool carpool = GetCarpool();
            foreach (Carpool carpool1 in new CarpoolPersistenceAsync().GetAllCarpools(carpool.Driver.Id).Result.Values)
            {
                if (carpool.Driver.Id == carpool1.Driver.Id)
                {
                    carpool.CarpoolId = carpool1.CarpoolId;
                    carpool.Passengerlist = carpool1.Passengerlist;
                    carpool.Passengerlist[carpool.Driver.Id].IsAccepted = true;
                }
            }

            //Act & Assert needs a carpool to be added
            Assert.IsNotNull(new CarpoolPersistenceAsync().UpdatePassenger(carpool.Driver.Id,carpool.CarpoolId).Result);
        }
        [TestMethod]
        public void CarpoolgetPassengersAdminFromDatabaseTest()
        {
            //Arrange
            Carpool carpool = GetCarpool();
            foreach (Carpool carpool1 in new CarpoolPersistenceAsync().GetAllCarpools(carpool.Driver.Id).Result.Values)
            {
                if (carpool.Driver.Id == carpool1.Driver.Id)
                {
                    carpool.CarpoolId = carpool1.CarpoolId;

                }
            }

            //Act & Assert needs a carpool to be added
            Assert.AreEqual(carpool.CarpoolId, new CarpoolPersistenceAsync().GetPassengersAdmin("Test").Result[0].Carpool.CarpoolId);
            Assert.AreEqual(carpool.Driver.Id, new CarpoolPersistenceAsync().GetPassengersAdmin("Test").Result[0].User.Id);
        }


        [TestMethod]
        public void CarpoolgetCarpoolsAllSearchFromDatabaseTest()
        {
            //Arrange
            Carpool carpool = GetCarpool();
            Carpool carpool2 = new Carpool();
            DateTime time = DateTime.Parse("13-01-2021 11:43:32");
            foreach (Carpool carpool1 in new CarpoolPersistenceAsync().GetAllCarpools(time,"Zealand").Result.Values)
            {
                if (carpool.Driver.Id == carpool1.Driver.Id)
                {
                    carpool2 = carpool1;

                }
            }
           
            //Act & Assert needs a carpool to be added to the database
           Assert.AreEqual(carpool.CarpoolId, carpool.CarpoolId);
            Assert.AreEqual(carpool.Branch.BranchId, carpool2.Branch.BranchId);
            Assert.AreEqual(carpool.Driver.Id, carpool2.Driver.Id);
        }




    }
}
