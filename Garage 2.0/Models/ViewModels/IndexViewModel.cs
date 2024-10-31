using System.ComponentModel;

namespace Garage_2._0.Models.ViewModels
{
    public class IndexViewModel
    {
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
    }
}
