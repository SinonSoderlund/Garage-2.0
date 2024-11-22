﻿using Garage_2._0.Utilities;
using System.ComponentModel;

namespace Garage_2._0.Models.ViewModels
{
    public class GarageStatisticsViewModel
    {
        [DisplayName("Vehicles by type")]
        public List<string> VehiclesByType { get; set; }
        [DisplayName("Total Number of Wheels")]

        public uint WheelTotal { get; set; }
        [DisplayName("Total price for parking")]

        public decimal ParkingTotalPrice { get; set; }
        [DisplayName("Total price for parking")]

        public string ParkingTotalPriceText { get; set; }
        public int TotalSpots { get; set; }
        public int OccupiedSpots { get; set; }
        public int AvailableSpots => TotalSpots - OccupiedSpots;
        public decimal OccupancyRate => TotalSpots == 0 ? 0 : (decimal)OccupiedSpots / TotalSpots * 100;
        public List<VehicleTypeStatistic> VehicleTypeStatistics { get; set; }

        public GarageStatisticsViewModel(List<string> vehiclesByType, uint wheelTotal, decimal parkingTotalPrice)
        {
            VehiclesByType = vehiclesByType;
            WheelTotal = wheelTotal;
            ParkingTotalPrice = parkingTotalPrice;
            ParkingTotalPriceText = ViewUtilities.PriceToString(ParkingTotalPrice); 
        }

        public GarageStatisticsViewModel() 
        {
            VehiclesByType = [string.Empty];
            WheelTotal = default;
            ParkingTotalPrice = default;
        }
    }
}
