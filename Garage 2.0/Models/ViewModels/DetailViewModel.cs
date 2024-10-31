using Garage_2._0.Models.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models.ViewModels
{
    public class DetailViewModel
    {
        public int Id { get; set; }
        [DisplayName("Number Of Wheels")]
        [Range(0, uint.MaxValue)]
        public uint Wheels { get; set; }

        [DisplayName("Time of Arrival")]
        public DateTime ArriveTime { get; set; }

        [StringLength(20)]
        public string Color { get; set; }

        [StringLength(20)]
        [DisplayName("Registration Number")]
        public string RegNr { get; set; }

        [StringLength(20)]
        public string Model { get; set; }

        [StringLength(20)]
        public string Brand { get; set; }

        [DisplayName("Type of Vehicle")]
        public VehicleType VehicleType { get; set; }


        /// <summary>
        /// Vehicle copy constructor
        /// </summary>
        /// <param name="vehicle">Vehicle to be copied</param>
        public DetailViewModel(Vehicle vehicle)
        {
            Id = vehicle.Id;
            Brand = vehicle.Brand;
            ArriveTime = vehicle.ArriveTime;
            RegNr = vehicle.RegNr;
            Color = vehicle.Color;
            Wheels = vehicle.Wheels;
            Model = vehicle.Model;
            VehicleType = vehicle.VehicleType;
        }
        /// <summary>
        /// Default constructor, sets strings to string.Empty and other fields to default.
        /// </summary>
        public DetailViewModel()
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
        public DetailViewModel(string brand, string regNr, string color, uint wheels, string model, VehicleType vehicleType)
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
        public DetailViewModel(DetailViewModel vehicle)
        {
            Brand = vehicle.Brand;
            ArriveTime = DateTime.Now;
            RegNr = vehicle.RegNr;
            Color = vehicle.Color;
            Wheels = vehicle.Wheels;
            Model = vehicle.Model;
            VehicleType = vehicle.VehicleType;
        }
    }
}
