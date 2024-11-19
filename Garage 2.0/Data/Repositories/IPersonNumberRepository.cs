namespace Garage_2._0.Data.Repositories
{
    public interface IPersonNumberRepository
    {

       public Task<bool> IsUnique(long personNr);
    }
}
