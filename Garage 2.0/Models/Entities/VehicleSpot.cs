namespace Garage_2._0.Models.Entities;

// Join entity type for the relationship
public class VehicleSpot
{
    public int VehicleId { get; set; }
    public int SpotId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    public Spot Spot { get; set; } = null!;
}