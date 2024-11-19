using Garage_2._0.Models.Entities;

namespace Garage_2._0.Models.ViewModels
{
    public class UserOverviewViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public int VehicleCount { get; set; }
        public decimal TotalCost { get; set; }
        public IEnumerable<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
