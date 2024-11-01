namespace Garage_2._0.Models.ViewModels
{
    public class GarageStatisticsViewModel
    {
        public List<string> VehiclesByType { get; set; }
        public uint WheelTotal { get; set; }
        public decimal ParkingTotalPrice { get; set; }

        public GarageStatisticsViewModel(List<string> vehiclesByType, uint wheelTotal, decimal parkingTotalPrice)
        {
            VehiclesByType = vehiclesByType;
            WheelTotal = wheelTotal;
            ParkingTotalPrice = parkingTotalPrice;
        }

        public GarageStatisticsViewModel() 
        {
            VehiclesByType = [string.Empty];
            WheelTotal = default;
            ParkingTotalPrice = default;
        }
    }
}
