
using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Data.Repositories
{
    public class PersonNumberRepository : IPersonNumberRepository
    {
        private readonly Garage_2_0Context _context;

        public PersonNumberRepository(Garage_2_0Context context)
        {
            _context = context;
        }

        public async Task<bool> IsUnique(long personNr)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PersonalNumber == personNr) == default;
        }
    }
}
