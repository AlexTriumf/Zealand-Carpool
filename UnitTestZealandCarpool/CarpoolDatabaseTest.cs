using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Zealand_Carpool.Models;
using Zealand_Carpool.Services;

namespace UnitTestZealandCarpool
{
    [TestClass]
    public class CarpoolDatabaseTest
    {
        public Carpool GetCarpool()
        {
           
            DateTime time = DateTime.Now;
            Carpool carpool = new Carpool();
            carpool.Branch = new Branch();
            carpool.Branch.BranchId = 1;
            carpool.Branch.BranchName = "Zealand Roskilde";
            carpool.Branch.BranchPostalCode = 4000;

            List<Address> addresses = new List<Address>();
            addresses.Add(new Address("Maglegårdsvej", "2", 4000, "Roskilde"));

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
            Assert.IsTrue(new CarpoolDatabaseAsync().AddCarpool(carpool).Result);
            // if theres 2 of the same carpools
            Assert.ThrowsException<InvalidOperationException>(() => new CarpoolDatabaseAsync().AddCarpool(carpool));
            
        }

        [TestMethod]
        public void CarpoolGetFromDatabaseTest()
        {
            //Arrange
            Carpool carpool = GetCarpool();
            List<Carpool> carpools = new List<Carpool>();
            foreach (Carpool carpool1 in new CarpoolDatabaseAsync().GetAllCarpools(carpool.Driver.Id).Result.Values)
            {
                if (carpool.Driver.Id == carpool1.Driver.Id)
                {
                    carpool.CarpoolId = carpool1.CarpoolId; 
                    carpools.Add(carpool1);
                }
            }
            //Act & Assert
            Assert.AreEqual(new CarpoolDatabaseAsync().GetCarpool(carpools[0].CarpoolId),carpool);

            //Assert.ThrowsException<InvalidOperationException>(() => new CarpoolDatabaseAsync().AddCarpool(carpool));

        }


        [TestMethod]
        public void CarpoolDeleteFromDatabaseTest()
        {
            //Arrange
            Carpool carpool = GetCarpool();
            
            foreach (Carpool carpool1 in new CarpoolDatabaseAsync().GetAllCarpools(carpool.Driver.Id).Result.Values)
            {
                if (carpool.Driver.Id == carpool1.Driver.Id)
                {
                    carpool.CarpoolId = carpool1.CarpoolId;
                    
                }
            }

            //Act & Assert
            Assert.IsTrue(new CarpoolDatabaseAsync().DeleteCarpool(carpool.CarpoolId).Result);
        }
    }
}
