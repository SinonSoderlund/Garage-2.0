namespace Garage_2._0.Models.ViewModels
{
    public class IndexViewModel
    {
        public VehicleType VehicleType { get; set; }

        public string RegNr { get; set; }

        public DateTime ArriveTime { get; set; }

        public TimeSpan ParkedDuration
        {
            get
            {
                return DateTime.Now - ArriveTime;
            }
        }
    }
}
