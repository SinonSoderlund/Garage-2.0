using Microsoft.AspNetCore.Identity;

namespace Garage_2._0.Models.Entities
{
    public class User : IdentityUser
    {
        public int PersonalNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
