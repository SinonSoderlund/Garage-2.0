using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage_2._0.Data;
using Garage_2._0.Models.Entities;
using Garage_2._0.Models.ViewModels;

namespace Garage_2._0.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly Garage_2_0Context _context;

        public VehiclesController(Garage_2_0Context context)
        {
            _context = context;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var model = await _context.Vehicle.Select(v => new IndexViewModel()
            {
                Id = v.Id,
                VehicleType = v.VehicleType,
                RegNr = v.RegNr,
                ArriveTime = v.ArriveTime
            }).ToListAsync();
            return View(model);
        }

        // Start Feature: Search area - mohammad
        public async Task<IActionResult> SearchByRegNumber(string searchField)
        {
            if (!string.IsNullOrEmpty(searchField))
            {
                var results = _context.Vehicle.Where(v => v.RegNr.Contains(searchField))
                .Select(v => new IndexViewModel()
                {
                    Id = v.Id,
                    RegNr = v.RegNr,
                    VehicleType = v.VehicleType,
                    ArriveTime = v.ArriveTime,
                });
                return View("Index", await results.ToListAsync());
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        // End Feature: Search area - mohammad


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
                if (await EnsureUnique(vehicle))
                {
                    Vehicle toAdd = new Vehicle(vehicle);
                    _context.Add(toAdd);
                    await _context.SaveChangesAsync();
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
                    ViewBag.VehicleTypes = new SelectList(Enum.GetValues(typeof(VehicleType))
                        .OfType<VehicleType>()
                        .Select(v => new { Id = v, Name = v.ToString() }), "Id", "Name", viewModel.VehicleType);
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

                    //parkedVehicle.ArriveTime = viewModel.ArriveTime;
                    parkedVehicle.RegNr = viewModel.RegNr;
                    parkedVehicle.Wheels = viewModel.Wheels;
                    parkedVehicle.Color = viewModel.Color;
                    parkedVehicle.Model = viewModel.Model;
                    parkedVehicle.Brand = viewModel.Brand;
                    parkedVehicle.VehicleType = viewModel.VehicleType;
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
                    throw;
                }
            }
         
            return View(viewModel);
        }
        private Task<bool> VehicleExistsAsync(int id)
        {
            return _context.Vehicle.AnyAsync(e => e.Id == id);
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
                ReceiptViewModel output = new ReceiptViewModel(vehicle, 16.64M);
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
