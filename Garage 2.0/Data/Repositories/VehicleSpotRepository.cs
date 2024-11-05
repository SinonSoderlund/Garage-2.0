using Garage_2._0.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Data.Repositories;

public interface IVehicleSpotRepository
{
    Task<IEnumerable<VehicleSpot>> GetAllVehicleSpots();
}

public class VehicleSpotRepository : IVehicleSpotRepository
{
    private readonly Garage_2_0Context _context;

    public VehicleSpotRepository(Garage_2_0Context context)
    {
        _context = context;
    }

    
    public async Task<IEnumerable<VehicleSpot>> GetAllVehicleSpots()
    {
        return await _context.VehicleSpots.ToListAsync();
    }
}