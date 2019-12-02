using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SE340.Areas.Identity.Data;
using SE340.Models;

namespace SE340.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly SE340UserContext _context;
        private readonly UserManager<SE340User> _userManager;
        public VehiclesController(SE340UserContext context, UserManager<SE340User> userContext)
        {
            _context = context;
            _userManager = userContext;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            SE340User user = await _userManager.FindByNameAsync(User.Identity.Name);
            var vehicles = new List<Vehicle>();
            ViewData["UserFavorites"] = user.FavoritedVehicles;

            vehicles = _context.Vehicles.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                vehicles = vehicles.Where(s => s.Make.Contains(searchString) || s.Model.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "make":
                    vehicles = vehicles.OrderBy(s => s.Make).ToList();
                    break;
                case "model":
                    vehicles = vehicles.OrderBy(s => s.Model).ToList();
                    break;
                case "weight":
                    vehicles = vehicles.OrderBy(s => s.Weight).ToList();
                    break;
                default:
                    vehicles = vehicles.OrderByDescending(s => s.Make).ToList();
                    break;
            }

            return View(vehicles);
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(m => m.ID == id);
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Make,Model,Drive,Engine_Layout,Num_cylinders,Displacement,Weight")] Vehicle vehicle)
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

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Make,Model,Drive,Engine_Layout,Num_cylinders,Displacement,Weight")] Vehicle vehicle)
        {
            if (id != vehicle.ID)
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
                    if (!VehicleExists(vehicle.ID))
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

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(m => m.ID == id);
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
            var vehicle = await _context.Vehicles.FindAsync(id);
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.ID == id);
        }
        
        [ActionName("Favorite")]
        public async Task<IActionResult> Favorite(int id)
        {
            var store = new UserStore<SE340User>(_context);
            var ctx = store.Context;

            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(m => m.ID == id);
            if (vehicle == null)
            {
                return NotFound();
            } else
            {
                Console.WriteLine(vehicle.Make);
            }

            SE340User user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }

            if (!user.FavoritedVehicles.Contains(vehicle))
            {
                user.FavoritedVehicles.Add(vehicle);

                await _userManager.UpdateAsync(user);
                ctx.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [ActionName("Unfavorite")]
        public async Task<IActionResult> Unfavorite(int id)
        {
            var store = new UserStore<SE340User>(_context);
            var ctx = store.Context;

            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(m => m.ID == id);
            if (vehicle == null)
            {
                return NotFound();
            }
            else
            {
                Console.WriteLine(vehicle.Make);
            }

            SE340User user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }

            if (user.FavoritedVehicles.Contains(vehicle))
            {
                user.FavoritedVehicles.Remove(vehicle);

                await _userManager.UpdateAsync(user);

                ctx.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        [ActionName("ViewFavorites")]
        public async Task<IActionResult> ViewFavorites()
        {
            SE340User user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewData["UserFavorites"] = user.FavoritedVehicles;

            return View(await _context.Vehicles.ToListAsync());
        }
    }
}
