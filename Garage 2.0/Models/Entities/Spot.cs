namespace Garage_2._0.Models.Entities;

public class Spot
{
    public int Id { get; set; }
    public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

    public List<VehicleSpot> VehicleSpots { get; set; } = new();
}