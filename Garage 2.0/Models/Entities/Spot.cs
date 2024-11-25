namespace Garage_2._0.Models.Entities;

public class Spot
{
    public int Id { get; set; }

    public ICollection<SpotAllocation> SpotAllocations { get; set; } // Navigation property for flexibility
}