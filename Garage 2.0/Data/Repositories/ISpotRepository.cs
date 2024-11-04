using Garage_2._0.Models.Entities;

namespace Garage_2._0.Data.Repositories;

public interface ISpotRepository
{
    Task<IEnumerable<Spot>> GetAvailableSpots();
    
    
    /// <summary>
    /// Function that examines Spots table to see if there is an available spot
    /// </summary>
    /// <returns>0 if no spots found, otherwise a positive integer that indicates available spot in Spots table </returns>
    Task<int> FindAvailableSpotId();
    Task <bool>AssignVehicleToSpot(int spotId, int vehicleId); // Assign a vehicle to a specific spot
}