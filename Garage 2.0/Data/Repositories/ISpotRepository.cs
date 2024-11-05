using Garage_2._0.Models.Entities;

namespace Garage_2._0.Data.Repositories;

public interface ISpotRepository
{
    Task<IEnumerable<Spot>> GetAllSpotsWithVehicles();
    
    /// <summary>
    /// Function that returns a list of all available spots in a table. Can use .Count to see how many available of
    /// returned list.
    /// </summary>
    /// <returns>A list of all available spots </returns>
    Task<IEnumerable<Spot>> GetAvailableSpots();
    
    
    /// <summary>
    /// Function that examines Spots table to see if there is an available spot. Returns the index of first found spot
    /// </summary>
    /// <returns>0 if no spots found, otherwise a positive integer that indicates available spot in Spots table </returns>
    Task<int> FindAvailableSpotId();
    
    /// <summary>
    /// Function that assigns a given vehicle to a spot by providing an empty spotId and the Id of the vehicle that
    /// should be assigned to that spot.
    /// </summary>
    /// <returns>true if the assignment operation is successful, otherwise false.</returns>
    Task <bool>AssignVehicleToSpot(int spotId, int vehicleId); // Assign a vehicle to a specific spot
}