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
    }
}
