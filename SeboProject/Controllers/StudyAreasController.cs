using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SeboProject.Data;
using SeboProject.Models;
using SeboProject.Utilities;

namespace SeboProject.Controllers
{
    public class StudyAreasController : Controller
    {
        private readonly SeboDbContext _context;

        public StudyAreasController(SeboDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentSearchString, string SearchString, int StudyAreaFilter, int? Page)
        {
            int PageSize = 14; // How many listed items per page definition
            var seboDbContext = _context.StudyArea;
            var BodyList = (from p in seboDbContext select p); // Item to be listed on the view

            if (!String.IsNullOrEmpty(sortOrder))
            {
                string[] parameter = sortOrder.Split(".");
                sortOrder = parameter[0];
                StudyAreaFilter = Int32.Parse(parameter[1]);
            }


            if (!String.IsNullOrEmpty(SearchString))
            {
                Page = 1;
                BodyList = BodyList.Where(p => p.StudyAreaName.Contains(SearchString));
            }
            else currentSearchString = SearchString;

            // Applying text filters on the table - It impacts in all dropboxes and lists

            //Initializing the queries that belong to the dropboxes
            var _StudyAreaFiltered = BodyList;

            // Applying filter over the dropbox query base
            if (StudyAreaFilter != 0)
            {
                _StudyAreaFiltered = from s in _StudyAreaFiltered where (s.StudyAreaId == StudyAreaFilter) select s;
            }

            /*
                Adjusts the BodyList according to the Institution selection 
                and the BranchName selection 
            */
            BodyList = _StudyAreaFiltered;

            /* Ordering before list on the view */
            BodyList = sortOrder == "StudyArea_asc" ?  BodyList.OrderByDescending(s => s.StudyAreaName): BodyList.OrderBy(s => s.StudyAreaName) ;

            /*
                Preparing DROPBOXLISTs
            */
            var _StudyArea = _StudyAreaFiltered.ToList();
            ViewData["StudyAreaFilter"] = new SelectList(_StudyArea, "StudyAreaId", "StudyAreaName");
            ViewData["SearchString"] = SearchString;

            /*
             * Adding actual dropboxes selection because in case of a column ordering (by clicking on the column name) 
             * the dropbox filtering values would be lost.
             */
            ViewData["StudyAreaOrder"] = (sortOrder == "StudyArea_asc" ? "StudyArea_desc" : "StudyArea_asc") + "." + StudyAreaFilter;

            return View(await Pagination<StudyArea>.CreateAsync(BodyList.AsNoTracking(), Page ?? 1, PageSize));

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
