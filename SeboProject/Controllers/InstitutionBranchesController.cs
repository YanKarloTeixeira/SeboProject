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
    public class InstitutionBranchesController : Controller
    {
        private readonly SeboDbContext _context;

        public InstitutionBranchesController(SeboDbContext context)
        {
            _context = context;
        }

        // GET: InstitutionBranches
        public async Task<IActionResult> Index()
        {
            var seboDbContext = _context.InstitutionBranch.Include(i => i.Institution);
            return View(await seboDbContext.ToListAsync());
        }

        // GET: InstitutionBranches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var institutionBranch = await _context.InstitutionBranch
                .Include(i => i.Institution)
                .FirstOrDefaultAsync(m => m.InstitutionBranchId == id);
            if (institutionBranch == null)
            {
                return NotFound();
            }

            return View(institutionBranch);
        }

        // GET: InstitutionBranches/Create
        public IActionResult Create()
        {
            ViewData["InstitutionId"] = new SelectList(_context.Institution, "InstitutionId", "InstitutionName");
            return View();
        }

        // POST: InstitutionBranches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstitutionBranchId,InstitutionBranchName,InstitutionId")] InstitutionBranch institutionBranch)
        {
            if (ModelState.IsValid)
            {
                _context.Add(institutionBranch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstitutionId"] = new SelectList(_context.Institution, "InstitutionId", "InstitutionName", institutionBranch.InstitutionId);
            return View(institutionBranch);
        }

        // GET: InstitutionBranches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var institutionBranch = await _context.InstitutionBranch.FindAsync(id);
            if (institutionBranch == null)
            {
                return NotFound();
            }
            ViewData["InstitutionId"] = new SelectList(_context.Institution, "InstitutionId", "InstitutionName", institutionBranch.InstitutionId);
            return View(institutionBranch);
        }

        // POST: InstitutionBranches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstitutionBranchId,InstitutionBranchName,InstitutionId")] InstitutionBranch institutionBranch)
        {
            if (id != institutionBranch.InstitutionBranchId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(institutionBranch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstitutionBranchExists(institutionBranch.InstitutionBranchId))
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
            ViewData["InstitutionId"] = new SelectList(_context.Institution, "InstitutionId", "InstitutionName", institutionBranch.InstitutionId);
            return View(institutionBranch);
        }

        // GET: InstitutionBranches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var institutionBranch = await _context.InstitutionBranch
                .Include(i => i.Institution)
                .FirstOrDefaultAsync(m => m.InstitutionBranchId == id);
            if (institutionBranch == null)
            {
                return NotFound();
            }

            return View(institutionBranch);
        }

        // POST: InstitutionBranches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var institutionBranch = await _context.InstitutionBranch.FindAsync(id);
            _context.InstitutionBranch.Remove(institutionBranch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstitutionBranchExists(int id)
        {
            return _context.InstitutionBranch.Any(e => e.InstitutionBranchId == id);
        }
    }
}
