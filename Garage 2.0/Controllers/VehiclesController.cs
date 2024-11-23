using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage_2._0.Data;
using Garage_2._0.Data.Repositories;
using Garage_2._0.Models.Entities;
using Garage_2._0.Models.ViewModels;
using Garage_2._0.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Garage_2._0.Controllers
{
    [Authorize]
    public class VehiclesController : Controller
    {
        private readonly Garage_2_0Context _context;
        private readonly ISpotRepository _spotRepository;
        private readonly decimal _price = 16.64M;
        private readonly IFeedbackMessageRepository _feedbackRepository;
        private readonly UserManager<User> _userManager;

        public VehiclesController(Garage_2_0Context context,
            ISpotRepository spotRepository,
            IFeedbackMessageRepository feedbackMessageRepository,
            UserManager<User> userManager)
        {
            _context = context;
            _spotRepository = spotRepository;
            _feedbackRepository = feedbackMessageRepository;
            _userManager = userManager;         
        }



        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var model = await _context.Vehicle
                .Include(v => v.VehicleType)
                .Include(v => v.User)
                .ToListAsync();

            ViewBag.VehicleTypes = await _context.VehicleTypes.ToListAsync();

            return View(new UnitedIndexViewCollection(
                model,
                _price,
                await _spotRepository.GetAvailableSpots(),
                UIVC_State.full,
                await _feedbackRepository.GetMessage()
            ));
        }


        // Start Feature: Search area
        public async Task<IActionResult> SearchByRegNumber(string searchField)
        {
            if (!string.IsNullOrEmpty(searchField))
            {
                var results = _context.Vehicle.Where(v => v.RegNr.Contains(searchField));
                return View("Index", new UnitedIndexViewCollection(await results.ToListAsync(), _price, await _spotRepository.GetAvailableSpots()));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        // End Feature: Search area

        // Start Feature: sort by date

        public async Task<IActionResult> SortByDateAscending()
        {
            var sortedVehicles = await _context.Vehicle
                .OrderBy(v => v.ArriveTime)
                .ToListAsync();

            return View("Index", new UnitedIndexViewCollection(sortedVehicles, _price, await _spotRepository.GetAvailableSpots()));
        }

        public async Task<IActionResult> SortByDateDescending()
        {
            var sortedVehicles = await _context.Vehicle
                .OrderByDescending(v => v.ArriveTime)
                .ToListAsync();

            return View("Index", new UnitedIndexViewCollection(sortedVehicles, _price, await _spotRepository.GetAvailableSpots()));
        }

        // End Feature: sort by date

        // Start: Filter by Vehicle type
        public async Task<IActionResult> FilterByVehicleType(string vehicleType)
        {
            if (!string.IsNullOrEmpty(vehicleType))
            {
                var filteredVehicles = _context.Vehicle
                    .Where(v => v.VehicleType.Name == vehicleType);
                return View("Index", new UnitedIndexViewCollection(await filteredVehicles.ToListAsync(), _price, await _spotRepository.GetAvailableSpots(), UIVC_State.filteredByType));
            }

            // If no filter is applied or invalid, show all vehicles
            return RedirectToAction(nameof(Index));
        }
        // End: Filter by Vehicle type

        // GET: Vehicles/ParkVehicle
        public async Task<IActionResult> ParkVehicle()
        {
            // Ensure user is logged in
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login page if not logged in
            }

            var vehicleTypes = await _context.VehicleTypes.ToListAsync();
            ViewBag.VehicleTypes = vehicleTypes.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.Name
            }).ToList();
            return View();
        }

        // Modify your existing ParkVehicle POST method to include additional validation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ParkVehicle(DetailViewModel vehicle)
        {
            // Ensure user is logged in
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // Check if the vehicle is already registered to this user
                var existingVehicle = await _context.Vehicle
                    .FirstOrDefaultAsync(v => v.RegNr == vehicle.RegNr && v.UserId == user.Id);

                if (existingVehicle != null)
                {
                    ModelState.AddModelError("", "This vehicle is already registered to you.");
                    return View(vehicle);
                }

                // Find available spot
                var availableSpotId = await _spotRepository.FindAvailableSpotId();
                if (availableSpotId == 0)
                {
                    ModelState.AddModelError("", "No parking spots available");
                    return View(vehicle);
                }

                if (await EnsureUnique(vehicle))
                {
                    // Create vehicle and associate with logged-in user
                    Vehicle newVehicle = new Vehicle(vehicle)
                    {
                        UserId = user.Id,
                        User = user
                    };

                    // Set the VehicleType
                    var vehicleType = await _context.VehicleTypes
                        .FirstOrDefaultAsync(vt => vt.Id == vehicle.VehicleTypeId);

                    if (vehicleType == null)
                    {
                        ModelState.AddModelError("", "Invalid vehicle type");
                        return View(vehicle);
                    }

                    newVehicle.VehicleTypeId = vehicleType.Id;
                    newVehicle.VehicleType = vehicleType;

                    _context.Add(newVehicle);
                    await _context.SaveChangesAsync();

                    // Assign vehicle to spot
                    await _spotRepository.AssignVehicleToSpot(availableSpotId, newVehicle.Id);

                    await _feedbackRepository.SetMessage(
                        new FeedbackMessage($"Vehicle (registration number {newVehicle.RegNr}) successfully parked!", AlertType.success));
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("regNr", "Registration number must be unique");
                }
            }

            // Repopulate vehicle types if model state is invalid
            var vehicleTypes = await _context.VehicleTypes.ToListAsync();
            ViewBag.VehicleTypes = vehicleTypes.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.Name
            }).ToList();

            return View(vehicle);
        }


        /// <summary>
        /// Function to ensure a vehicle to be added is unique, not ideal implementation since verification isnt enforced, but its a start
        /// </summary>
        /// <param name="toVerify">DetailViewModel to be verified</param>
        /// <param name="Id">Id of edited object, leave empty for newly created objects</param>
        /// <returns></returns>
        public async Task<bool> EnsureUnique(DetailViewModel toVerify, int Id = -1)
        {
            toVerify.Id = Id;
            return await _context.Vehicle.FirstOrDefaultAsync(v => (v.RegNr == toVerify.RegNr) && (v.Id != toVerify.Id)) == default;
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                await _feedbackRepository.SetMessage(new FeedbackMessage($"Invalid request. Vehicle not found.", AlertType.danger));
                return RedirectToAction(nameof(Index));
            }

            var userId = _userManager.GetUserId(User); 
            var isAdmin = User.IsInRole("Admin"); 

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.Id == id && (m.UserId == userId || isAdmin));

            if (vehicle == null)
            {
                await _feedbackRepository.SetMessage(new FeedbackMessage($"You are not authorized to View this vehicle.", AlertType.danger));
                return RedirectToAction(nameof(Index));
            }

            DetailViewModel output = new DetailViewModel(vehicle);
            return View(output);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                await _feedbackRepository.SetMessage(new FeedbackMessage($"Invalid request. Vehicle not found.", AlertType.danger));
                return RedirectToAction(nameof(Index));
            }

            var userId = _userManager.GetUserId(User); 
            var isAdmin = User.IsInRole("Admin"); 

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.Id == id && (m.UserId == userId || isAdmin));

            if (vehicle == null)
            {
                await _feedbackRepository.SetMessage(new FeedbackMessage($"You are not authorized to Edit this vehicle.", AlertType.danger));
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new DetailViewModel(vehicle);
            return View(viewModel);
        }

        // POST: Vehicles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DetailViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User); 
            var isAdmin = User.IsInRole("Admin"); 

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.Id == id && (m.UserId == userId || isAdmin));

            if (vehicle == null)
            {
                await _feedbackRepository.SetMessage(new FeedbackMessage($"You are not authorized to Edit this vehicle.", AlertType.danger));
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                if (!await EnsureUnique(viewModel, id))
                {
                    ModelState.AddModelError("RegNr", "Registration number must be unique");
                    return View(viewModel);
                }

                try
                {
                    vehicle.UpdateVehicle(viewModel);
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                    await _feedbackRepository.SetMessage(new FeedbackMessage($"Vehicle (registration number {vehicle.RegNr}) successfully edited!", AlertType.success));
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await VehicleExistsAsync(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(viewModel);
        }

        private async Task<bool> VehicleExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        // GET: Vehicles/CheckOut/5
        public async Task<IActionResult> CheckOut(int? id)
        {
            if (id == null)
            {
                await _feedbackRepository.SetMessage(new FeedbackMessage($"Invalid request. Vehicle not found.", AlertType.danger));
                return RedirectToAction(nameof(Index));
            }

            var userId = _userManager.GetUserId(User); 
            var isAdmin = User.IsInRole("Admin"); 

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.Id == id && (m.UserId == userId || isAdmin));

            if (vehicle == null)
            {
                await _feedbackRepository.SetMessage(new FeedbackMessage($"You are not authorized to Check-out this vehicle.", AlertType.danger));
                return RedirectToAction(nameof(Index));
            }
            DetailViewModel output = new DetailViewModel(vehicle);
            return View(output);
        }

        // POST: Vehicles/CheckOut/5
        [HttpPost, ActionName("CheckOut")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOutConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User); 
            var isAdmin = User.IsInRole("Admin"); 

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.Id == id && (m.UserId == userId || isAdmin));

            if (vehicle == null)
            {
                await _feedbackRepository.SetMessage(new FeedbackMessage($"You are not authorized to Check-out this vehicle.", AlertType.danger));
                return RedirectToAction(nameof(Index));
            }

            _context.Vehicle.Remove(vehicle);
            await _feedbackRepository.SetMessage(new FeedbackMessage($"Vehicle (registration number {vehicle.RegNr}) Checked Out", AlertType.info));
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Vehicles/GetReceipt/5
        [HttpPost, ActionName("GetReceipt")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetReceipt(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle != null)
            {
                // Find the spot containing this vehicle
                var spot = await _context.Spots
                    .Include(s => s.Vehicles)
                    .FirstOrDefaultAsync(s => s.VehicleId == id);

                if (spot != null)
                {
                    // Clear the VehicleId from the spot
                    spot.VehicleId = null;

                    // Remove the vehicle from the Vehicles collection
                    if (spot.Vehicles != null)
                    {
                        spot.Vehicles.Remove(vehicle);
                    }

                    _context.Spots.Update(spot);
                }
                _context.Vehicle.Remove(vehicle);
                await _context.SaveChangesAsync();
                ReceiptViewModel output = new ReceiptViewModel(vehicle, _price);
                await _feedbackRepository.SetMessage(new FeedbackMessage($"Vehicle (registration number {vehicle.RegNr}) Checked Out", AlertType.info));
                return View(output);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.Id == id);
        }
    }
}
