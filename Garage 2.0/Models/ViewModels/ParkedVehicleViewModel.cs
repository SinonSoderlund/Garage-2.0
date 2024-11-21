using Garage_2._0.Models.Entities;
using System.ComponentModel;

namespace Garage_2._0.Models.ViewModels
{
    public class ParkedVehicleViewModel
    {
        public int Id { get; set; }


        [DisplayName("Owner ID")]
        public string OwnerId { get; set; }

        [DisplayName("Owner Full Name")]
        public string OwnerFullName { get; set; }

        [DisplayName("Vehicle Type")]
        public string VehicleType { get; set; }

        [DisplayName("Registration Number")]
        public string RegNr { get; set; }

        [DisplayName("Parked on Spots")]
        public ICollection<Spot> Spots { get; set; }

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
    }
}
