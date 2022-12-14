using Lab_UnitTestingMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab_UnitTestingMVC.Data
{
    public class ParkingContext : DbContext
    {
        public virtual DbSet<Vehicle> Vehicles { get; set; }

        public virtual DbSet<Pass> Passes { get; set; }

        public virtual DbSet<ParkingSpot> ParkingSpots { get; set; }

        public virtual DbSet<Reservation> Reservations { get; set; }

    }
}
