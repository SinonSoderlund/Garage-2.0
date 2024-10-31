using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Garage_2._0.Models.Entities;

namespace Garage_2._0.Models.ViewModels
{
    public class ReceiptViewModel
    {
        public int Id { get; set; }
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

        public TimeSpan ParkedDuration
        {
            get
            {
                return DateTime.Now - ArriveTime;
            }
        }

        [DisplayName("Time of Departure")]
        public DateTime CheckoutTime { get; set; }


        [DisplayName("Total Price")]
        public Decimal Price { get; set; }

        /// <summary>
        /// Vehicle copy constructor
        /// </summary>
        /// <param name="vehicle">Vehicle to be copied</param>
        public ReceiptViewModel(Vehicle vehicle, decimal price)
        {
            Id = vehicle.Id;
            Brand = vehicle.Brand;
            ArriveTime = vehicle.ArriveTime;
            RegNr = vehicle.RegNr;
            Color = vehicle.Color;
            Wheels = vehicle.Wheels;
            Model = vehicle.Model;
            VehicleType = vehicle.VehicleType;
            CheckoutTime = DateTime.Now;
            Price = (ParkedDuration.Seconds / 3600) * price; 
        }
    }
}
