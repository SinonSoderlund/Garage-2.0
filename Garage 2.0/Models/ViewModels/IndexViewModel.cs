using Garage_2._0.Models.Entities;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Garage_2._0.Models.ViewModels
{
    public class IndexViewModel
    {
        public int Id { get; set; }
        public string Owner { get; set; } //added owner prop 

        [DisplayName("Type of Vehicle")]
        public VehicleType VehicleType { get; set; }

        [DisplayName("Registration Number")]
        public string RegNr { get; set; }

        [DisplayName("Time of Arrival")]
        public DateTime ArriveTime { get; set; }

        
        [DisplayName("Time Parked")]
        public TimeSpan ParkedDuration
        {
            get
            {
                return DateTime.Now - ArriveTime;
            }
        }
        
        public string ParkedDurationFormatted
        {
            get
            {
                return $"{(ParkedDuration.Days > 0 ? $"{ParkedDuration.Days}d | " : "")}" +
                       $"{ParkedDuration.Hours:D2}h | {ParkedDuration.Minutes:D2}m | {ParkedDuration.Seconds:D2}s";
            }
        }

        /// <summary>
        /// Vehicle copy constructor
        /// </summary>
        /// <param name="vehicle">Vehicle to be copied</param>
        public IndexViewModel(Vehicle vehicle)
        {
            Id = vehicle.Id;
            ArriveTime = vehicle.ArriveTime;
            RegNr = vehicle.RegNr;
            VehicleType = vehicle.VehicleType;
            Owner = vehicle.User?.FirstName + " " + vehicle.User?.LastName;
        }
        /// <summary>
        /// Default constructor, sets strings to string.Empty and other fields to default.
        /// </summary>
        public IndexViewModel()
        {
            ArriveTime = default;
            RegNr = string.Empty;
            VehicleType = default;
        }
    }
}
