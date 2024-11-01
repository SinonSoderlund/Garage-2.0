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

    public async Task< IEnumerable<Spot>> GetAvailableSpots()
    {
        return await _context.Spots.Where(s => s.VehicleId == null).ToListAsync();
    }
}