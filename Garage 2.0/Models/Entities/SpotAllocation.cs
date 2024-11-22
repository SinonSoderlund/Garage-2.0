namespace Garage_2._0.Models.Entities
{
    public class SpotAllocation
    {
        public int Id { get; set; }
        public int SpotId { get; set; }
        public Spot Spot { get; set; } // Navigation property

        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } // Navigation property

        public decimal Fraction { get; set; } // E.g., 1.0 for full spot, 0.5 for half a spot
    }
}
