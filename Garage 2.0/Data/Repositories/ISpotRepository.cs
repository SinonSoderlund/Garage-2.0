using Garage_2._0.Models.Entities;

namespace Garage_2._0.Data.Repositories;

public interface ISpotRepository
{
    Task<IEnumerable<Spot>> GetAvailableSpots();
}