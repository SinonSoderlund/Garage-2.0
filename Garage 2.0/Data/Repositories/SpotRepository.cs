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
        return await _context.Spots
            .Where(s => s.SpotAllocations.Sum(sa => sa.Fraction) + vehicleSize <= 1.0m)
            .Select(s => s.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Spot>> GetAllSpots()
    {
        return await _context.Spots
            .Include(s => s.SpotAllocations)
            .ToListAsync();
    }

}