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
    public class StudyAreasController : Controller
    {
        private readonly SeboDbContext _context;

        public StudyAreasController(SeboDbContext context)
        {
            _context = context;
        }

        // GET: StudyAreas
        public async Task<IActionResult> Index()
        {
            return View(await _context.StudyArea.ToListAsync());
        }

        // GET: StudyAreas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studyArea = await _context.StudyArea
                .FirstOrDefaultAsync(m => m.StudyAreaId == id);
            if (studyArea == null)
            {
                return NotFound();
            }

            return View(studyArea);
        }

        // GET: StudyAreas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudyAreas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudyAreaId,StudyAreaName")] StudyArea studyArea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studyArea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studyArea);
        }

        // GET: StudyAreas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studyArea = await _context.StudyArea.FindAsync(id);
            if (studyArea == null)
            {
                return NotFound();
            }
            return View(studyArea);
        }

        // POST: StudyAreas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudyAreaId,StudyAreaName")] StudyArea studyArea)
        {
            if (id != studyArea.StudyAreaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studyArea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudyAreaExists(studyArea.StudyAreaId))
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
            return View(studyArea);
        }

        // GET: StudyAreas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studyArea = await _context.StudyArea
                .FirstOrDefaultAsync(m => m.StudyAreaId == id);
            if (studyArea == null)
            {
                return NotFound();
            }

            return View(studyArea);
        }

        // POST: StudyAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studyArea = await _context.StudyArea.FindAsync(id);
            _context.StudyArea.Remove(studyArea);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudyAreaExists(int id)
        {
            return _context.StudyArea.Any(e => e.StudyAreaId == id);
        }
    }
}
