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
    public class InstitutionBranchesController : Controller
    {
        private readonly SeboDbContext _context;

        public InstitutionBranchesController(SeboDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentSearchString, string SearchString, int InstitutionFilter, int BranchNameFilter, int? Page)
        {
            int PageSize = 14; // How many listed items per page definition
            var seboDbContext = _context.InstitutionBranch.Include(c => c.Institution);
            var BodyList = (from p in seboDbContext select p); // Item to be listed on the view

            if (!String.IsNullOrEmpty(sortOrder))
            {
                string[] parameter = sortOrder.Split(".");
                sortOrder = parameter[0];
                InstitutionFilter = Int32.Parse(parameter[1]);
                BranchNameFilter = Int32.Parse(parameter[2]);
            }


            if (!String.IsNullOrEmpty(SearchString))
            {
                Page = 1;
                BodyList = BodyList.Where(p => p.Institution.InstitutionName.Contains(SearchString) || p.InstitutionBranchName.Contains(SearchString));
            }
            else currentSearchString = SearchString;

            // Applying text filters on the table - It impacts in all dropboxes and lists

            //Initializing the queries that belong to the dropboxes
            var _InstitutionsFiltered = (from i in _context.Institution select i).OrderBy(i=>i.InstitutionName);
            var _BranchNamesFiltered = BodyList;

            // Applying filter over the dropbox query base
            if (InstitutionFilter != 0)
            {
                _BranchNamesFiltered = from p in _BranchNamesFiltered where (p.InstitutionId == InstitutionFilter) select p;
            }

            /*
                Adjusts the BodyList according to the Institution selection 
                and the BranchName selection 
            */
            BodyList = _BranchNamesFiltered;
            if (BranchNameFilter != 0) BodyList = _BranchNamesFiltered.Where(p => p.InstitutionBranchId == BranchNameFilter).Distinct();
            if (BodyList.Count() < 1) BodyList = _BranchNamesFiltered; // In case no match for Institution+BranchName 



            //_BranchNamesFiltered = OrderingPostalCodes.Do(_BranchNamesFiltered, sortOrder);
            /* Ordering before list on the view */
            BodyList = OrderingInstitutionBranches.Do(BodyList, sortOrder);

            /*
                Preparing DROPBOXLISTs
            */
            var _Institutions = _InstitutionsFiltered.ToList();
            var _BranchNames = (from p in _BranchNamesFiltered orderby p.InstitutionBranchName
                                select new { p.InstitutionBranchId, p.InstitutionBranchName }).ToList().Distinct();

            ViewData["InstitutionFilter"] = new SelectList(_Institutions, "InstitutionId", "InstitutionName");
            ViewData["BranchNameFilter"] = new SelectList(_BranchNames, "InstitutionBranchId", "InstitutionBranchName");
            /* END of (Preparing DROPBOXLISTs) */

            ViewData["SearchString"] = SearchString;
            /*
             * Adding actual dropboxes selection because in case of a column ordering (by clicking on the column name) 
             * the dropbox filtering values would be lost.
             */
            ViewData["InstitutionOrder"] =OrderingInstitutionBranches.NewOrder(sortOrder,"Institution")  + "." + InstitutionFilter + "." + BranchNameFilter;
            ViewData["BranchNameOrder"] = OrderingInstitutionBranches.NewOrder(sortOrder,"BranchName") + "." + InstitutionFilter + "." + BranchNameFilter;

            return View(await Pagination<InstitutionBranch>.CreateAsync(BodyList.AsNoTracking(), Page ?? 1, PageSize));

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
