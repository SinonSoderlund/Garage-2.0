﻿using System;
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

namespace Garage_2._0.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly Garage_2_0Context _context;
        private readonly ISpotRepository _spotRepository;
        private readonly decimal _price = 16.64M;
        private FeedbackBannerViewModel _feedbackBannerMessage;

        public VehiclesController(Garage_2_0Context context, ISpotRepository spotRepository)
        {
            _context = context;
            _spotRepository = spotRepository;
            _feedbackBannerMessage = null!;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var model = await _context.Vehicle.ToListAsync();
            var availableSpots = await _spotRepository.GetAvailableSpots();
            ViewBag.AvailableSpots = availableSpots.Count();

            return View(new UnitedIndexViewCollection(model, _price));
        }
    
        // Start Feature: Search area
        public async Task<IActionResult> SearchByRegNumber(string searchField)
        {
            if (!string.IsNullOrEmpty(searchField))
            {
                var results = _context.Vehicle.Where(v => v.RegNr.Contains(searchField));
                var availableSpots = await _spotRepository.GetAvailableSpots();
                ViewBag.AvailableSpots = availableSpots.Count();
                return View("Index", new UnitedIndexViewCollection(await results.ToListAsync(), _price));
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
            var availableSpots = await _spotRepository.GetAvailableSpots();
            ViewBag.AvailableSpots = availableSpots.Count();
            return View("Index", new UnitedIndexViewCollection(sortedVehicles, _price));
        }

        public async Task<IActionResult> SortByDateDescending()
        {
            var sortedVehicles = await _context.Vehicle
                .OrderByDescending(v => v.ArriveTime)
                .ToListAsync();
            var availableSpots = await _spotRepository.GetAvailableSpots();
            ViewBag.AvailableSpots = availableSpots.Count();
            return View("Index", new UnitedIndexViewCollection(sortedVehicles, _price));
        }

        // End Feature: sort by date

        // Start: Filter by Vehicle type
        public async Task<IActionResult> FilterByVehicleType(string vehicleType)
        {
            if (!string.IsNullOrEmpty(vehicleType) && Enum.TryParse(vehicleType, out VehicleType type))
            {
                var filteredVehicles = _context.Vehicle
                    .Where(v => v.VehicleType == type);
                var availableSpots = await _spotRepository.GetAvailableSpots();
                ViewBag.AvailableSpots = availableSpots.Count();
                return View("Index", new UnitedIndexViewCollection(await filteredVehicles.ToListAsync(), _price, UIVC_State.filteredByType));
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
        public IActionResult ParkVehicle()
        {
            return View();
        }

        // POST: Vehicles/ParkVehicle
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
            public async Task<IActionResult> ParkVehicle(DetailViewModel vehicle)
            {
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
                        Vehicle toAdd = new Vehicle(vehicle);
                        _context.Add(toAdd);
                        await _context.SaveChangesAsync();
                        await _spotRepository.AssignVehicleToSpot(availableSpotId, toAdd.Id);
                        TempData["Message"] = "Vehicle successfully parked."; // feedback message
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
                ReceiptViewModel output = new ReceiptViewModel(vehicle,_price);
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
