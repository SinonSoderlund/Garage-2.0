using Garage_2._0.Utilities;
using System.ComponentModel;

namespace Garage_2._0.Models.ViewModels
{
    public class GarageStatisticsViewModel
    {
        [DisplayName("Vehicles by type")]
        public List<VehicleTypeCount> VehiclesByType { get; set; }

        [DisplayName("Total Number of Wheels")]
        public uint WheelTotal { get; set; }

        [DisplayName("Total price for parking")]
        public decimal ParkingTotalPrice { get; set; }

        [DisplayName("Total price for parking")]
        public string ParkingTotalPriceText { get; set; }

        public int TotalSpots { get; set; }
        public int OccupiedSpots { get; set; }

        private int _availableSpots;
        public int AvailableSpots
        {
            get => TotalSpots - OccupiedSpots;
            set => _availableSpots = value;
        }

        private decimal _occupancyRate;
        public decimal OccupancyRate
        {
            get => TotalSpots == 0 ? 0 : (decimal)OccupiedSpots / TotalSpots * 100;
            set => _occupancyRate = value;
        }

        public TimeSpan AverageParkedTime { get; set; }
        public string AverageParkedTimeFormatted =>
            $"{(int)AverageParkedTime.TotalHours}h {AverageParkedTime.Minutes}m";
        public string ParkingTotalPriceFormatted => ParkingTotalPrice.ToString("C");

        // Constructor for the new format
        public GarageStatisticsViewModel()
        {
            VehiclesByType = new List<VehicleTypeCount>();
            WheelTotal = default;
            ParkingTotalPrice = default;
        }

        // Constructor for the old format (List<string>)
        public GarageStatisticsViewModel(List<string> vehiclesByTypeStrings, uint wheelTotal, decimal parkingTotalPrice)
        {
            VehiclesByType = vehiclesByTypeStrings.Select(s => {
                var parts = s.Split(':');
                return new VehicleTypeCount
                {
                    TypeName = parts[0].Trim(),
                    Count = int.Parse(parts[1].Trim())
                };
            }).ToList();

            WheelTotal = wheelTotal;
            ParkingTotalPrice = parkingTotalPrice;
            ParkingTotalPriceText = ViewUtilities.PriceToString(ParkingTotalPrice);
        }
    }

    public class VehicleTypeCount
    {
        public string TypeName { get; set; }
        public int Count { get; set; }
    }
}