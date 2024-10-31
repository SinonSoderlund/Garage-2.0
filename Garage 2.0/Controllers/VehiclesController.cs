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

        // Start Feature: sort by date - mohammad

        public async Task<IActionResult> SortByDateAscending()
        {
            var sortedVehicles = await _context.Vehicle
                .OrderBy(v=>v.ArriveTime)
                .ToListAsync();
            return View("Index", sortedVehicles);
        }

        public async Task<IActionResult> SortByDateDescending()
        {
            var sortedVehicles = await _context.Vehicle
                .OrderByDescending(v => v.ArriveTime)
                .ToListAsync();
            return View("Index", sortedVehicles);
        }

        // End Feature: sort by date - mohammad



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
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Wheels,ArriveTime,Color,RegNr,Model,Brand,VehicleType")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicle.Remove(vehicle);
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
