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

    public async Task<IEnumerable<Spot>> GetAvailableSpots()
    {
        return await _context.Spots.Where(s => s.VehicleId == null).ToListAsync();
    }
    
    public async Task<int> FindAvailableSpotId()
    {
        var availableSpotId = await _context.Spots
            .Where(s => s.VehicleId == null) // See if spot is available
            .Select(s => s.Id)
            .FirstOrDefaultAsync(); // returns first found empty spot in database where value is null, 0 if no spot is found
        
        return availableSpotId;
    }

    public async Task<bool> AssignVehicleToSpot(int spotId, int vehicleId)
    {
        var spot = await _context.Spots.FindAsync(spotId); // gets a spot from Spots table with given id
        
        // Check if the spot exists and that it doesn't contain a VehicleId (which means a vehicle is parked here).
        if (spot != null && spot.VehicleId == null) 
        {
            spot.VehicleId = vehicleId; // assign vehicle to the spot
            await _context.SaveChangesAsync(); // save changes to database
            return true;
        }

        return false;
    }

}