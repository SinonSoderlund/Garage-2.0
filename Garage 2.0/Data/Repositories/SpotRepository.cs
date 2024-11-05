using Garage_2._0.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Data.Repositories;

public class SpotRepository : ISpotRepository
{
    private readonly Garage_2_0Context _context;

    public SpotRepository(Garage_2_0Context context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Spot>> GetAllSpotsWithVehicles()
    {
        return await _context.Spots
            .Include(s => s.Vehicles)  // Directly include Vehicles
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Spot>> GetAvailableSpots()
    {
        // Return spots that do not have any vehicle associated with them
        return await _context.Spots
            .Where(s => !s.VehicleSpots.Any()) // Check if there are no VehicleSpot entries for this spot
            .ToListAsync();
    }

    public async Task<int> FindFirstAvailableTripleSpot()
    {
        // Get all spots and order them by their ID
        var spots = await _context.Spots
            .OrderBy(s => s.Id)
            .ToListAsync();

        // Loop through all the spots in the spots-list. -2...
        // because it's uneccesary and program will crash...
        // trying to get an out-of-range index (spots[i + 2].Vehicles.Count == 0)
        for (int i = 0; i < spots.Count - 2; i++)
        {
            if (spots[i].Vehicles.Count == 0 && spots[i + 1].Vehicles.Count == 0 && spots[i + 2].Vehicles.Count == 0)
            {
                // if we find 3 adjecant spots where no vehicles are parked, we return the middle point
                return spots[i + 1].Id;
            }
            
        }

        // return 0 if no Id is found.
        return 0; 
    }

    public async Task<int> FindAvailableSpotId()
    {
        var availableSpotId = await _context.Spots
            .Where(s => s.Vehicles.Count == 0) 
            .Select(s => s.Id)
            .FirstOrDefaultAsync(); // Return the first available spot id, or 0 if none found
        
        return availableSpotId;
    }

    public async Task<int> FindFirstAvailableMotorcycleSpotId()
    {
        var firstAvailableMcSpotId = await _context.Spots
                
            // Looks for a spot where count is <= 3 and where the Vehicle type is MC    
            .Where(s => s.Vehicles.Count(v => v.VehicleType == VehicleType.Motorcycle) <= 3)
            
            // Selects the id if above criteria is met
            .Select(s => s.Id)
            
            // Will return first available spot id
            .FirstOrDefaultAsync();
        
        // if no spot id is to be found, then 0 is returned.
        return firstAvailableMcSpotId;
    }

    public async Task<bool> AssignVehicleToSpot(int spotId, int vehicleId)
    {
        // Create a new accociation in the Join Table where SpotId points to the VehicleId
        var vehicleSpot = new VehicleSpot
        {
            SpotId = spotId,
            VehicleId = vehicleId
        };

        _context.VehicleSpots.Add(vehicleSpot); // Adds the accociation to the Join Table
        await _context.SaveChangesAsync(); // Save the changes
        return true;
    }
}