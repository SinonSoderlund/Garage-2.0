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
    
    public async Task<int> FindSpotForVehicle(decimal vehicleSize)
    {
        var bestSpot = await _context.Spots
            // only gets spots where the new vehicleSize + allocated space doesn't exceed 1.0
            .Where(s => s.SpotAllocations.Sum(sa => sa.Fraction) + vehicleSize <= 1.0m)
            .Select(s => new
            {
                SpotId = s.Id,
                TotalAllocation = s.SpotAllocations.Sum(sa => sa.Fraction)
            })
            // sort the previous select statement, so that the best spot (closest to 1.0) gets on top.
            .OrderByDescending(s => s.TotalAllocation)
            .ThenBy(s => s.SpotId) // if it's a tie then the slot with lower id nr gets picked
            .FirstOrDefaultAsync();

        return bestSpot?.SpotId?? 0; // return 0 if no suitable spot is found.
    }
    
    public async Task<IEnumerable<Spot>> GetAllSpots()
    {
        return await _context.Spots
            .Include(s => s.SpotAllocations)
            .ToListAsync();
    }
}