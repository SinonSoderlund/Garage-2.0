using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage_2._0.Data;
using Garage_2._0.Models.Entities;

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
            return View(await _context.Vehicle.ToListAsync());
        }

        // Start Feature: Search area - mohammad
        public async Task<IActionResult> SearchByRegNumber(string searchField)
        {
            if (!string.IsNullOrEmpty(searchField))
            {
                var results = _context.Vehicle.Where(v => v.RegNr.Contains(searchField))
                .Select(v => new Vehicle
                {
                    Id = v.Id,
                    RegNr = v.RegNr,
                    Brand = v.Brand,
                    Model = v.Model,
                    VehicleType = v.VehicleType,
                    Color = v.Color,
                    Wheels = v.Wheels,
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

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Wheels,ArriveTime,Color,RegNr,Model,Brand,VehicleType")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
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
