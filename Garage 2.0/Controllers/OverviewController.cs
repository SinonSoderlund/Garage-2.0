using Garage_2._0.Data;
using Garage_2._0.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OverviewController : Controller
    {
        private readonly Garage_2_0Context _context;

        public OverviewController(Garage_2_0Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IActionResult Index(string searchTerm)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u => u.FirstName.Contains(searchTerm) || u.LastName.Contains(searchTerm));
            }

            var members = query
                .Select(u => new UserOverviewViewModel
                {
                    UserId = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    VehicleCount = u.Vehicles.Count()
                }).ToList();

            return View(members);
        }

        [HttpGet]
        public IActionResult Details(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound(); // Handle invalid userId
            }

            var user = _context.Users
                .Include(u => u.Vehicles)
                .ThenInclude(v => v.VehicleType) // Include VehicleType to avoid null reference
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return NotFound(); // Handle case where user is not found
            }

            var vehicles = user.Vehicles.Select(v => new UserVehicleViewModel
            {
                RegNr = v.RegNr,
                Model = v.Model,
                Brand = v.Brand,
                Color = v.Color,
                VehicleType = v.VehicleType != null ? v.VehicleType.Name : "Unknown",
                ArriveTime = v.ArriveTime
            }).ToList();

            ViewBag.UserName = $"{user.FirstName} {user.LastName}";
            return View(vehicles);
        }



    }
}
