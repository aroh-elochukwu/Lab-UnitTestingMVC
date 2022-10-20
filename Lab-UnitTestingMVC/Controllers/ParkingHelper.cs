using Lab_UnitTestingMVC.Data;
using Lab_UnitTestingMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab_UnitTestingMVC.Controllers
{
    public class ParkingHelper 
    {
        private ParkingContext parkingContext;

        public ParkingHelper(ParkingContext context)

        {

            parkingContext = context;

        }

        public Pass CreatePass(string purchaser, bool premium, int capacity)

        {

            Pass newPass = new Pass(purchaser, capacity);           

            newPass.Premium = premium;

            parkingContext.Passes.Add(newPass);

            parkingContext.SaveChanges();

            return newPass;

        }

        public ParkingSpot CreateParkingSpot()

        {

            ParkingSpot newSpot = new ParkingSpot();

            newSpot.Occupied = false;

            parkingContext.ParkingSpots.Add(newSpot);

            return newSpot;

        }

        public Pass BuyPass(string purchaser,int capacity, HashSet<Vehicle> vehicles )
        {
            Pass newPass = new Pass(purchaser, capacity);
            newPass.Premium = true;
            if (vehicles.Count! > capacity)
            {
                foreach (Vehicle vehicle in vehicles)
                {
                    newPass.Vehicles.Add(vehicle);
                }
            }

            return newPass;
        }

        public void AddVehicleToPass(string passHolderName, string vehicleLicence)
        {
            try
            {
                Pass currPass = parkingContext.Passes.First(p => p.Purchaser == passHolderName);
                Vehicle currVehicle = parkingContext.Vehicles.First(v => v.License == vehicleLicence);
                currPass.Vehicles.Add(currVehicle);
                if (currPass.Capacity >= currPass.Vehicles.Count)
                {
                    parkingContext.SaveChanges();
                }
                else
                {
                    throw new IndexOutOfRangeException("Pass capacity is out of range");
                }

            }
            catch
            {
                throw new NullReferenceException("Vehicle or pass not found");
            }

        }

        public void BookParkingSpot()
        {

        }

    }


}
