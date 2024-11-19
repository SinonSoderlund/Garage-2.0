using Garage_2._0.Data;
using Garage_2._0.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Controllers
{
    public class UsersController : Controller
    {
        private readonly Garage_2_0Context _context;

        public UsersController(Garage_2_0Context context)
        {
            _context = context;
        }
        // GET: Users Overview
        public async Task<IActionResult> Overview(string searchField, string vehicleTypeFilter)
        {
            // Fetch all users
            var query = _context.Users.Include(u => u.Vehicles).AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(searchField))
            {
                query = query.Where(u => u.FirstName.Contains(searchField) || u.LastName.Contains(searchField));
            }

            // Filter by vehicle type if applicable
            if (int.TryParse(vehicleTypeFilter, out int vehicleTypeId))
            {
                query = query.Where(u => u.Vehicles.Any(v => v.VehicleType.Id == vehicleTypeId));
            }


            var users = await query.ToListAsync();

            // Map data to the ViewModel
            var model = users.Select(user => new UserOverviewViewModel
            {
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                VehicleCount = user.Vehicles.Count,
                Vehicles = user.Vehicles
            });

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Vehicles)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var model = new UserOverviewViewModel
            {
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                VehicleCount = user.Vehicles.Count,
                Vehicles = user.Vehicles
            };

            return View(model);
        }

    }
}
