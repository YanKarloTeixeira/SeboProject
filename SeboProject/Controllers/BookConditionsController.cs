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
    public class BookConditionsController : Controller
    {
        private readonly SeboDbContext _context;

        public BookConditionsController(SeboDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string sortOrder, string currentSearchString, string SearchString, int BookConditionFilter, int? Page)
        {
            int PageSize = 14; // How many listed items per page definition
            var seboDbContext = _context.BookCondition;
            var BodyList = (from p in seboDbContext select p); // Item to be listed on the view

            if (!String.IsNullOrEmpty(sortOrder))
            {
                string[] parameter = sortOrder.Split(".");
                sortOrder = parameter[0];
                BookConditionFilter = Int32.Parse(parameter[1]);
            }


            if (!String.IsNullOrEmpty(SearchString))
            {
                Page = 1;
                BodyList = BodyList.Where(p => p.Condition.Contains(SearchString));
            }
            else currentSearchString = SearchString;

            // Applying text filters on the table - It impacts in all dropboxes and lists

            //Initializing the queries that belong to the dropboxes
            var _BookConditionFiltered = BodyList;

            // Applying filter over the dropbox query base
            if (BookConditionFilter != 0)
            {
                _BookConditionFiltered = from s in _BookConditionFiltered where (s.BookConditionId == BookConditionFilter) select s;
            }

            /*
                Adjusts the BodyList according to the Institution selection 
                and the BranchName selection 
            */
            BodyList = _BookConditionFiltered;

            /* Ordering before list on the view */
            BodyList = sortOrder == "BookCondition_asc" ? BodyList.OrderByDescending(s => s.Condition) : BodyList.OrderBy(s => s.Condition);

            /*
                Preparing DROPBOXLISTs
            */
            var _BookCondition = _BookConditionFiltered.ToList();
            ViewData["BookConditionFilter"] = new SelectList(_BookCondition, "BookConditionId", "Condition");
            ViewData["SearchString"] = SearchString;

            /*
             * Adding actual dropboxes selection because in case of a column ordering (by clicking on the column name) 
             * the dropbox filtering values would be lost.
             */
            ViewData["BookConditionOrder"] = (sortOrder == "BookCondition_asc" ? "BookCondition_desc" : "BookCondition_asc") + "." + BookConditionFilter;

            return View(await Pagination<BookCondition>.CreateAsync(BodyList.AsNoTracking(), Page ?? 1, PageSize));

        }
        // GET: BookConditions
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.BookCondition.ToListAsync());
        //}

        // GET: BookConditions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookCondition = await _context.BookCondition
                .FirstOrDefaultAsync(m => m.BookConditionId == id);
            if (bookCondition == null)
            {
                return NotFound();
            }

            return View(bookCondition);
        }

        // GET: BookConditions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BookConditions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookConditionId,Condition")] BookCondition bookCondition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookCondition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookCondition);
        }

        // GET: BookConditions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookCondition = await _context.BookCondition.FindAsync(id);
            if (bookCondition == null)
            {
                return NotFound();
            }
            return View(bookCondition);
        }

        // POST: BookConditions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookConditionId,Condition")] BookCondition bookCondition)
        {
            if (id != bookCondition.BookConditionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookCondition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookConditionExists(bookCondition.BookConditionId))
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
            return View(bookCondition);
        }

        // GET: BookConditions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookCondition = await _context.BookCondition
                .FirstOrDefaultAsync(m => m.BookConditionId == id);
            if (bookCondition == null)
            {
                return NotFound();
            }

            return View(bookCondition);
        }

        // POST: BookConditions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookCondition = await _context.BookCondition.FindAsync(id);
            _context.BookCondition.Remove(bookCondition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookConditionExists(int id)
        {
            return _context.BookCondition.Any(e => e.BookConditionId == id);
        }
    }
}
