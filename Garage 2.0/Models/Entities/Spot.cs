namespace Garage_2._0.Models.Entities;

public class Spot
{
    public int Id { get; set; }
    public int? VehicleId { get; set; } // nullable, null represents empty spot.
    public Vehicle? Vehicles { get; set; } // navigation property.
    public int SpotNumber { get; set; }
}