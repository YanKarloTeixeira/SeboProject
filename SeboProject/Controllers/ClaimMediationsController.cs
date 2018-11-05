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
    public class ClaimMediationsController : Controller
    {
        private readonly SeboDbContext _context;

        public ClaimMediationsController(SeboDbContext context)
        {
            _context = context;
        }

        // GET: ClaimMediations
        public async Task<IActionResult> Index()
        {
            var seboDbContext = _context.ClaimMediation.Include(c => c.Claim);
            return View(await seboDbContext.ToListAsync());
        }

        // GET: ClaimMediations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claimMediation = await _context.ClaimMediation
                .Include(c => c.Claim)
                .FirstOrDefaultAsync(m => m.ClaimMediationId == id);
            if (claimMediation == null)
            {
                return NotFound();
            }

            return View(claimMediation);
        }

        // GET: ClaimMediations/Create
        public IActionResult Create()
        {
            ViewData["ClaimId"] = new SelectList(_context.Claim, "ClaimId", "Description");
            return View();
        }

        // POST: ClaimMediations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClaimMediationId,Description,Action,ClaimId")] ClaimMediation claimMediation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(claimMediation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClaimId"] = new SelectList(_context.Claim, "ClaimId", "Description", claimMediation.ClaimId);
            return View(claimMediation);
        }

        // GET: ClaimMediations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claimMediation = await _context.ClaimMediation.FindAsync(id);
            if (claimMediation == null)
            {
                return NotFound();
            }
            ViewData["ClaimId"] = new SelectList(_context.Claim, "ClaimId", "Description", claimMediation.ClaimId);
            return View(claimMediation);
        }

        // POST: ClaimMediations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClaimMediationId,Description,Action,ClaimId")] ClaimMediation claimMediation)
        {
            if (id != claimMediation.ClaimMediationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(claimMediation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClaimMediationExists(claimMediation.ClaimMediationId))
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
            ViewData["ClaimId"] = new SelectList(_context.Claim, "ClaimId", "Description", claimMediation.ClaimId);
            return View(claimMediation);
        }

        // GET: ClaimMediations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claimMediation = await _context.ClaimMediation
                .Include(c => c.Claim)
                .FirstOrDefaultAsync(m => m.ClaimMediationId == id);
            if (claimMediation == null)
            {
                return NotFound();
            }

            return View(claimMediation);
        }

        // POST: ClaimMediations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var claimMediation = await _context.ClaimMediation.FindAsync(id);
            _context.ClaimMediation.Remove(claimMediation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClaimMediationExists(int id)
        {
            return _context.ClaimMediation.Any(e => e.ClaimMediationId == id);
        }
    }
}
