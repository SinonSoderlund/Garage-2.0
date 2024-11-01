using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Garage_2._0.Models.Entities;
using Garage_2._0.Utilities;

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

        public string ParkedDurationText
        {
            get
            {
                return ParkTime();
            }
        }

        [DisplayName("Time of Departure")]
        public DateTime CheckoutTime { get; set; }


        [DisplayName("Total Price")]
        public Decimal Price { get; set; }

        public string PriceText { get; set; }

        /// <summary>
        /// Copies vehicle data into recept and then fills out recept fields
        /// </summary>
        /// <param name="vehicle">Vehicle to be copied</param>
        /// <param name="price">hourly price for parking</param>

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
            Price = ViewUtilities.getPrice(ParkedDuration.Ticks, price);
            PriceText = Price.ToString("C");
        }

        /// <summary>
        /// Outputs time parked as string
        /// </summary>
        /// <returns></returns>
        private string ParkTime()
        {
            TimeSpan t = ParkedDuration;
            return $"{t.Days} Days, {t.Hours} Hours, {t.Minutes} Minutes, {t.Seconds} Seconds.";
        }



        /// <summary>
        /// Default constructor, sets strings to string.Empty and other fields to default.
        /// </summary>
        public ReceiptViewModel()
        {
            Brand = string.Empty;
            ArriveTime = default;
            RegNr = string.Empty;
            Color = string.Empty;
            Wheels = default;
            Model = string.Empty;
            VehicleType = default;
            CheckoutTime = default;
            Price = default;
            PriceText = string.Empty;
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
        public ReceiptViewModel(string brand, string regNr, string color, uint wheels, string model, VehicleType vehicleType, decimal price)
        {
            Brand = brand;
            ArriveTime = DateTime.Now;
            RegNr = regNr;
            Color = color;
            Wheels = wheels;
            Model = model;
            VehicleType = vehicleType;
            CheckoutTime = DateTime.Now;
            Price = ViewUtilities.getPrice(ParkedDuration.Ticks, price);
            PriceText = Price.ToString("C");

        }
        /// <summary>
        /// Vehicle copy constructor
        /// </summary>
        /// <param name="receipt">receipt to be copied</param>
        public ReceiptViewModel(ReceiptViewModel receipt)
        {
            Id = receipt.Id;
            Brand = receipt.Brand;
            ArriveTime = DateTime.Now;
            RegNr = receipt.RegNr;
            Color = receipt.Color;
            Wheels = receipt.Wheels;
            Model = receipt.Model;
            VehicleType = receipt.VehicleType;
            CheckoutTime = receipt.CheckoutTime;
            Price = receipt.Price;
            PriceText = receipt.PriceText;
        }
    }
}
