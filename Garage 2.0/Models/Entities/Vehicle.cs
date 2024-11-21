using Garage_2._0.Controllers;
using Garage_2._0.Models.ViewModels;
using NuGet.Versioning;
using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public uint Wheels { get; set; }

        public DateTime ArriveTime { get; set; }

        [StringLength(20)]
        public string Color { get; set; }

        [StringLength(20)]
        public string RegNr { get; set; }

        [StringLength(20)]
        public string Model { get; set; }

        [StringLength(20)]
        public string Brand { get; set; }

        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; } // nav prop

        public ICollection<Spot> Spots { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }


        /// <summary>
        /// Takes the data from the DetailViewModel and construts a vehicle from it
        /// </summary>
        /// <param name="viewModel">A valid DetaiViewModel</param>
        public Vehicle(DetailViewModel viewModel)
        {
            Brand = viewModel.Brand;
            ArriveTime = DateTime.Now;
            RegNr = viewModel.RegNr;
            Color = viewModel.Color;
            Wheels = viewModel.Wheels;
            Model = viewModel.Model;
            //VehicleType = viewModel.VehicleType;
        }
        /// <summary>
        /// Default constructor, sets strings to string.Empty and other fields to default.
        /// </summary>
        public Vehicle()
        {
            Brand = string.Empty;
            ArriveTime = default;
            RegNr = string.Empty;
            Color = string.Empty;
            Wheels = default;
            Model = string.Empty;
            VehicleType = default;
        }
        /// <summary>
        /// Parameter constructor
        /// </summary>
        /// <param name="brand">Vehicle brand, max 20 chars</param>
        /// <param name="regNr">Vehicle registration number, max 20 chars</param>
        /// <param name="color">vehicle color, max 20 chars</param>
        /// <param name="wheels">Vehicles number of wheels</param>
        /// <param name="model">Vehicle model/make, max 20 chars</param>
        /// <param name="vehicleType">Vehicles vheicle type</param>
        public Vehicle(string brand, string regNr, string color, uint wheels, string model, VehicleType vehicleType)
        {
            Brand = brand;
            ArriveTime = DateTime.Now;
            RegNr = regNr;
            Color = color;
            Wheels = wheels;
            Model = model;
            VehicleType = vehicleType;
        }
        /// <summary>
        /// Vehicle copy constructor
        /// </summary>
        /// <param name="vehicle">Vehicle to be copied</param>
        public Vehicle(Vehicle vehicle)
        {
            Brand = vehicle.Brand;
            ArriveTime = DateTime.Now;
            RegNr = vehicle.RegNr;
            Color = vehicle.Color;
            Wheels = vehicle.Wheels;
            Model = vehicle.Model;
            VehicleType = vehicle.VehicleType;
        }

        /// <summary>
        /// Vehicle update function
        /// </summary>
        /// <param name="model">model data to be inserted into this vehicle</param>
        public void UpdateVehicle(DetailViewModel model)
        {
            Brand = model.Brand;
            RegNr = model.RegNr;
            Color = model.Color;
            Wheels = model.Wheels;
            Model = model.Model;
            //VehicleType = model.VehicleType;
        }
    }

}
