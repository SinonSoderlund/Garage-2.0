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

namespace Garage_2._0.Controllers
{
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
            var model = await _context.Vehicle.ToListAsync();
            ViewBag.VehicleTypes = await _context.VehicleTypes.ToListAsync();
            return View(new UnitedIndexViewCollection(model, _price, await _spotRepository.GetAvailableSpots(), UIVC_State.full, await _feedbackRepository.GetMessage()));
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


        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }
            DetailViewModel output = new DetailViewModel(vehicle);

            return View(output);
        }

        // GET: Vehicles/ParkVehicle
        public async Task<IActionResult> ParkVehicle()
        {
            var vehicleTypes = await _context.VehicleTypes.ToListAsync();
            ViewBag.VehicleTypes = vehicleTypes.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(), // vehicleId as value
                Text = v.Name // name as display text
            }).ToList();
            return View();
        }

        // POST: Vehicles/ParkVehicle
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ParkVehicle(DetailViewModel vehicle)
        {
            var userId = _userManager.GetUserId(User);
            var vehicleTypes = await _context.VehicleTypes.ToListAsync();
            ViewBag.VehicleTypes = vehicleTypes.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(), // vehicleId as value
                Text = v.Name // name as display text
            }).ToList();

            if (ModelState.IsValid)
            {
                var availableSpotId = await _spotRepository.FindAvailableSpotId();
                if (availableSpotId == 0) // No available spot was found if this is true
                {
                    ModelState.AddModelError("", "no spots available");
                    return View(vehicle);
                }

                if (await EnsureUnique(vehicle))
                {
                    Vehicle newVehicle = new Vehicle(vehicle)
                    {
                        Wheels = vehicle.Wheels,
                        ArriveTime = vehicle.ArriveTime,
                        Color = vehicle.Color,
                        RegNr = vehicle.RegNr,
                        Model = vehicle.Model,
                        Brand = vehicle.Brand,
                        VehicleTypeId = vehicle.VehicleTypeId,
                        UserId = userId
                    };
                    _context.Add(newVehicle);
                    await _context.SaveChangesAsync();

                 
                    await _feedbackRepository.SetMessage(new FeedbackMessage($"Vehicle (registration number {newVehicle.RegNr}) sucessfully parked!", AlertType.success));
                    return RedirectToAction(nameof(Index));
                }
                else
                    ModelState.AddModelError("regNr", "Registration number must be unique");
            }
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
        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
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

            if (ModelState.IsValid)
            {
                if (!await EnsureUnique(viewModel, id))
                {
                    ModelState.AddModelError("RegNr", "Registration number must be unique");
                    return View(viewModel);
                }

                try
                {
                    // Find and update the existing vehicle’s properties
                    var parkedVehicle = await _context.Vehicle.FindAsync(id);
                    if (parkedVehicle == null)
                    {
                        return NotFound();
                    }
                    parkedVehicle.UpdateVehicle(viewModel);
                    _context.Update(parkedVehicle);
                    await _context.SaveChangesAsync();
                    await _feedbackRepository.SetMessage(new FeedbackMessage($"Vehicle (registration number {parkedVehicle.RegNr}) sucessfully edited!", AlertType.success));
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

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> CheckOut(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }
            DetailViewModel output = new DetailViewModel(vehicle);

            return View(output);
        }

        // POST: Vehicles/CheckOut/5
        [HttpPost, ActionName("CheckOut")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOutConfirmed(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicle.Remove(vehicle);
                await _feedbackRepository.SetMessage(new FeedbackMessage($"Vehicle (registration number {vehicle.RegNr}) Checked Out", AlertType.info));
            }

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
