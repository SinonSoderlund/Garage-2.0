namespace Garage_2._0.Models.Entities;

public class Spot
{
    public int Id { get; set; }
    public int? VehicleId { get; set; } // nullable, null represents empty spot.
    public Vehicle? Vehicle { get; set; } // navigation property.
}