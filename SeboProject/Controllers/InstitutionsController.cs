﻿using System;
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
    public class InstitutionsController : Controller
    {
        private readonly SeboDbContext _context;

        public InstitutionsController(SeboDbContext context)
        {
            _context = context;
        }

        // GET: Institutions
        public async Task<IActionResult> Index(string sortOrder, string currentSearchString, string SearchString,  int? Page)
        {
            int PageSize = 14; // How many listed items per page definition
            var seboDbContext = _context.Institution;
            var BodyList = (from p in seboDbContext select p); // Item to be listed on the view

            if (!String.IsNullOrEmpty(SearchString))
            {
                Page = 1;
                BodyList = BodyList.Where(p => p.InstitutionName.Contains(SearchString));                                                                               
            }
            else currentSearchString = SearchString;

            /* Ordering before list on the view */
            BodyList = sortOrder=="Institution_desc" ? BodyList.OrderBy(p=>p.InstitutionName) : BodyList.OrderByDescending(p => p.InstitutionName);



            ViewData["SearchString"] = SearchString;
            /*
             * Adding actual dropboxes selection because in case of a column ordering (by clicking on the column name) 
             * the dropbox filtering values would be lost.
             */
            ViewData["InstitutionOrder"] = sortOrder == "Institution_asc" ? "Institution_desc" : "Institution_asc";

            return View(await Utilities.Pagination<Institution>.CreateAsync(BodyList.AsNoTracking(), Page ?? 1, PageSize));

        }

        // GET: Institutions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var institution = await _context.Institution
                .FirstOrDefaultAsync(m => m.InstitutionId == id);
            if (institution == null)
            {
                return NotFound();
            }

            return View(institution);
        }

        // GET: Institutions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Institutions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstitutionId,InstitutionName")] Institution institution)
        {
            if (ModelState.IsValid)
            {
                _context.Add(institution);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(institution);
        }

        // GET: Institutions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var institution = await _context.Institution.FindAsync(id);
            if (institution == null)
            {
                return NotFound();
            }
            return View(institution);
        }

        // POST: Institutions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstitutionId,InstitutionName")] Institution institution)
        {
            if (id != institution.InstitutionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(institution);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstitutionExists(institution.InstitutionId))
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
            return View(institution);
        }

        // GET: Institutions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var institution = await _context.Institution
                .FirstOrDefaultAsync(m => m.InstitutionId == id);
            if (institution == null)
            {
                return NotFound();
            }

            return View(institution);
        }

        // POST: Institutions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var institution = await _context.Institution.FindAsync(id);
            _context.Institution.Remove(institution);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstitutionExists(int id)
        {
            return _context.Institution.Any(e => e.InstitutionId == id);
        }
    }
}
