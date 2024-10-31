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
            Price = getPrice(ParkedDuration.Ticks, price);
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
        /// Calculates the cost of parking
        /// </summary>
        /// <param name="time">time in timespand ticks for parking duration</param>
        /// <param name="price">hourly price of parking</param>
        /// <returns>cost of parking</returns>
        private decimal getPrice(long time, decimal price)
        {
            decimal dTime = Convert.ToDecimal(time);
            decimal dDevide = Convert.ToDecimal(TimeSpan.TicksPerHour);
            return (dTime/dDevide)*price;
        }

    }
}
