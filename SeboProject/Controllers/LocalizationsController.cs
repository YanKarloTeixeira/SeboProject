using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SeboProject.Data;
using SeboProject.Models;

namespace SeboProject.Controllers
{
    public class LocalizationsController : Controller
    {
        private readonly SeboDbContext _context;

        public LocalizationsController(SeboDbContext context)
        {
            _context = context;
        }

        // GET: Localizations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Localization.ToListAsync());
        }

        // GET: Localizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localization = await _context.Localization
                .FirstOrDefaultAsync(m => m.LocalizationId == id);
            if (localization == null)
            {
                return NotFound();
            }

            return View(localization);
        }

        // GET: Localizations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Localizations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocalizationId,PostalCode,PlaceName,Province")] Localization localization)
        {
            if (ModelState.IsValid)
            {
                _context.Add(localization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(localization);
        }

        // GET: Localizations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localization = await _context.Localization.FindAsync(id);
            if (localization == null)
            {
                return NotFound();
            }
            return View(localization);
        }

        // POST: Localizations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LocalizationId,PostalCode,PlaceName,Province")] Localization localization)
        {
            if (id != localization.LocalizationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(localization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocalizationExists(localization.LocalizationId))
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
            return View(localization);
        }

        // GET: Localizations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localization = await _context.Localization
                .FirstOrDefaultAsync(m => m.LocalizationId == id);
            if (localization == null)
            {
                return NotFound();
            }

            return View(localization);
        }

        // POST: Localizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var localization = await _context.Localization.FindAsync(id);
            _context.Localization.Remove(localization);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocalizationExists(int id)
        {
            return _context.Localization.Any(e => e.LocalizationId == id);
        }
    }
}
