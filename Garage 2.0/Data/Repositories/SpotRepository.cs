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
        return await _context.Spots
          .Include(s => s.Vehicles)//make sure we include vehicles collection
          .Where(s => s.VehicleId == null)
          .ToListAsync();
    }

    public async Task<int> FindAvailableSpotId()
    {
        // First ensure we have spots
        if (!await _context.Spots.AnyAsync())
        {
            await InitializeSpots();
        }

        // Find first spot without a vehicle assigned
        var availableSpot = await _context.Spots
            .Include(s => s.Vehicles)
            .FirstOrDefaultAsync(s => s.VehicleId == null);

        return availableSpot?.Id ?? 0;
    } 

    public async Task<bool> AssignVehicleToSpot(int spotId, int vehicleId)
    {
        // Include Vehicles collection when fetching the spot
        var spot = await _context.Spots
            .Include(s => s.Vehicles)
            .FirstOrDefaultAsync(s => s.Id == spotId);

        // Check if the spot exists and that it doesn't contain a VehicleId (which means a vehicle is parked here).
        if (spot != null && spot.VehicleId == null) 
        {
            spot.VehicleId = vehicleId; // assign vehicle to the spot

            var vehicle = await _context.Vehicle.FindAsync(vehicleId);
            if (vehicle != null)
            {
                if (spot.Vehicles == null)
                {
                    spot.Vehicles = new List<Vehicle>();
                }
                spot.Vehicles.Add(vehicle);
            }
            await _context.SaveChangesAsync(); // save changes to database
            return true;
        }

        return false;
    }
    private async Task InitializeSpots()
    {
        if (!await _context.Spots.AnyAsync())
        {
            // Create initial spots (e.g., 8 spots)
            var spots = Enumerable.Range(1, 8)
                .Select(i => new Spot
                {
                    VehicleId = null,
                    Vehicles = new List<Vehicle>()
                })
                .ToList();

            _context.Spots.AddRange(spots);
            await _context.SaveChangesAsync();
        }
    }
}