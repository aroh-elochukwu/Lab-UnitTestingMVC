using Lab_UnitTestingMVC.Controllers;
using Lab_UnitTestingMVC.Data;
using Lab_UnitTestingMVC.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Lab_UnitTestingMVCTests
{
    [TestClass]
    public class ParkingHelperTests
    {
        private ParkingContext _context;
        private ParkingHelper ParkingHelper; 
        
        
        public ParkingHelperTests()
        {
            // Pass Mock Data
            var passData = new List<Pass>
            {
                new Pass("Elo", 3){ID = 1, Premium = false},
                new Pass("Mylo", 3){ID = 2, Premium = false },
                new Pass("Bane", 3){ID = 3, Premium = false},
                new Pass("Nedu", 3) {ID = 4, Premium = false},
                new Pass("Usman Dan Fodio", 3) {ID = 5, Premium = false},
            }.AsQueryable();

            var passMockDbSet = new Mock<DbSet<Pass>>();

            passMockDbSet.As<IQueryable<Pass>>().Setup(m => m.Provider).Returns(passData.Provider);
            passMockDbSet.As<IQueryable<Pass>>().Setup(m => m.Expression).Returns(passData.Expression);
            passMockDbSet.As<IQueryable<Pass>>().Setup(m => m.ElementType).Returns(passData.ElementType);
            passMockDbSet.As<IQueryable<Pass>>().Setup(m => m.GetEnumerator()).Returns(passData.GetEnumerator());

            // Parking Spot Mock Data
            var PSData = new List<ParkingSpot>
            {
                //Reservation
                new ParkingSpot{ID = 1, Occupied = false},
                new ParkingSpot{ID = 2, Occupied = false},
                new ParkingSpot{ID = 3, Occupied = false},
                new ParkingSpot{ID = 4, Occupied = false},
                new ParkingSpot{ID = 5, Occupied = false},
            }.AsQueryable();

            var PSmockDbSet = new Mock<DbSet<ParkingSpot>>();

            PSmockDbSet.As<IQueryable<ParkingSpot>>().Setup(m => m.Provider).Returns(PSData.Provider);
            PSmockDbSet.As<IQueryable<ParkingSpot>>().Setup(m => m.Expression).Returns(PSData.Expression);
            PSmockDbSet.As<IQueryable<ParkingSpot>>().Setup(m => m.ElementType).Returns(PSData.ElementType);
            PSmockDbSet.As<IQueryable<ParkingSpot>>().Setup(m => m.GetEnumerator()).Returns(PSData.GetEnumerator());

            // Vehicle Mock Data
            var VehicleData = new List<Vehicle>
            {
                new Vehicle{ID = 1, Parked = false, License = "SRX-567"},
                new Vehicle{ID = 2, Parked = false, License = "G5R-12G"},
                new Vehicle{ID = 3, Parked = false, License = "ABC-DEL"},
                new Vehicle{ID = 4, Parked = false, License = "419-TEL"},
                new Vehicle{ID = 5, Parked = false, License = "JAV-OOP"},
            }.AsQueryable();

            var VehicleMockDbSet = new Mock<DbSet<Vehicle>>();

            VehicleMockDbSet.As<IQueryable<Vehicle>>().Setup(m => m.Provider).Returns(VehicleData.Provider);
            VehicleMockDbSet.As<IQueryable<Vehicle>>().Setup(m => m.Expression).Returns(VehicleData.Expression);
            VehicleMockDbSet.As<IQueryable<Vehicle>>().Setup(m => m.ElementType).Returns(VehicleData.ElementType);
            VehicleMockDbSet.As<IQueryable<Vehicle>>().Setup(m => m.GetEnumerator()).Returns(VehicleData.GetEnumerator());

            // Reservation Mock Data
            var ReservationData = new List<Reservation>
            {
                new Reservation{ID = 1},
                new Reservation{ID = 2},
                new Reservation{ID = 3},
                new Reservation{ID = 4},
                new Reservation{ID = 5},
            }.AsQueryable();

            var ReservationMockDbSet = new Mock<DbSet<Reservation>>();

            ReservationMockDbSet.As<IQueryable<Reservation>>().Setup(m => m.Provider).Returns(ReservationData.Provider);
            ReservationMockDbSet.As<IQueryable<Reservation>>().Setup(m => m.Expression).Returns(ReservationData.Expression);
            ReservationMockDbSet.As<IQueryable<Reservation>>().Setup(m => m.ElementType).Returns(ReservationData.ElementType);
            ReservationMockDbSet.As<IQueryable<Reservation>>().Setup(m => m.GetEnumerator()).Returns(ReservationData.GetEnumerator());

            var mockContext = new Mock<ParkingContext>();
            mockContext.Setup(p => p.Passes).Returns(passMockDbSet.Object);
            mockContext.Setup(v => v.ParkingSpots).Returns(PSmockDbSet.Object);
            mockContext.Setup(ps => ps.Vehicles).Returns(VehicleMockDbSet.Object);
            mockContext.Setup(r => r.Reservations).Returns(ReservationMockDbSet.Object);
            _context = mockContext.Object;
            ParkingHelper = new ParkingHelper(mockContext.Object);
        }


        [DataRow(1)]
        [TestMethod]
        public void CreatePass_ValidPassParam_GeneratesANewPass(int suposedPassCount)
        {
            ParkingHelper.CreatePass("test", true, 5);
            int actualPassCount = _context.Passes.Count();

            Assert.AreEqual(suposedPassCount, actualPassCount);
        }

        [DataRow("aba", true, 5)]
        [DataRow("aa", true, 2)]
        [DataRow("sfhvisbjk", true, 0)]
        [TestMethod]
        public void CreatePass_InvalidInput_ThrowExceptionWhenPurchaserCharIsOutOfRange(string purchaser, bool prem, int capacity)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                ParkingHelper.CreatePass(purchaser, prem, capacity);
            });
        }

        [DataRow("InvalidPurchaser", "123-234")]
        [TestMethod]
        public void AddVehicleToPass_InvalidInput_ThrowExceptionWhenPassHolderOrVehicleNotFound(string passRecipientName, string vehicleLicence)
        {
            Assert.ThrowsException<NullReferenceException>(() =>
            {
                ParkingHelper.AddVehicleToPass(passRecipientName, vehicleLicence);
            });
        }

        [DataRow("Jane", "129-291")]
        [TestMethod]
        public void AddVehicleToPass_OutOfCapacity_ThrowExceptionWhenUserAddAVehicleForOutOfCapacityPass(string passRecipientName, string vehicleLicence)
        {
            Assert.ThrowsException<IndexOutOfRangeException>(() =>
            {
                ParkingHelper.AddVehicleToPass(passRecipientName, vehicleLicence);
            });
        }
    }
}